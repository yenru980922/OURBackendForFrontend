using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.Dtos.Product;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendForFrontend.Controllers
{

    public class ProductKeywordDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int KeywordId { get; set; }
        public string KeywordName { get; set; }
    }

    /// <summary>
    /// 商品
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public ProductsPagingDto Get([FromQuery] ProductFilter filterData)
        {
            var products = filterData.ProductDetailsCategoryId == 0
                ? _db.BookProducts.AsNoTracking()
                     .Include(x => x.Book)
                     .Include(x => x.Product)
                     .Include(x => x.Publisher)
                     .Include(x => x.Product.DetailsCategory)
                     .Include(x => x.Product.ProductKeywords)
                     .ThenInclude(pk => pk.Keyword)

                : _db.BookProducts.AsNoTracking()
                     .Include(x => x.Book)
                     .Include(x => x.Product)
                     .Include(x => x.Publisher)
                     .Include(x => x.Product.DetailsCategory)
                     .Include(x => x.Product.ProductKeywords)
                     .ThenInclude(pk => pk.Keyword)
                     .Where(x => x.Product.DetailsCategoryId == filterData.ProductDetailsCategoryId);



            if (!string.IsNullOrEmpty(filterData.Keyword))
            {
                switch (filterData.BookSearch)
                {
                    case 10:
                        products = products.Where(x =>
                                                 x.Product.Name.Contains(filterData.Keyword)
                                              || x.Book.Author.Contains(filterData.Keyword)
                                              || x.Publisher.Name.Contains(filterData.Keyword)
                                              || x.Product.ProductKeywords.Select(x => x.Keyword.Name.Contains(filterData.Keyword)).Any(b => b)); break;
                    case 20: products = products.Where(x => x.Product.Name.Contains(filterData.Keyword)); break;
                    case 30: products = products.Where(x => x.Book.Author.Contains(filterData.Keyword)); break;
                    case 40: products = products.Where(x => x.Publisher.Name.Contains(filterData.Keyword)); break;
                }
            }
            if (!string.IsNullOrEmpty(filterData.PhysicalBook)
                && !string.IsNullOrEmpty(filterData.EBook)
                && filterData.PhysicalBook == "on"
                && filterData.EBook == "on")
            {
                products = products.Where(x => x.Product.Category.Contains("實體書")
                                            || x.Product.Category.Contains("電子書"));
            }
            else if (!string.IsNullOrEmpty(filterData.PhysicalBook) && filterData.PhysicalBook == "on")
            {
                products = products.Where(x => x.Product.Category.Contains("實體書"));
            }
            else if (!string.IsNullOrEmpty(filterData.EBook) && filterData.EBook == "on")
            {
                products = products.Where(x => x.Product.Category.Contains("電子書"));
            }

            products = products.Where(x =>
                                     (x.Product.RealPrice.HasValue 
                                   && x.Product.RealPrice.Value >= filterData.PriceRangeStart 
                                   && x.Product.RealPrice.Value <= filterData.PriceRangeEnd )
                                      ||  
                                    (!x.Product.RealPrice.HasValue 
                                   && x.Product.Price >= filterData.PriceRangeStart 
                                   && x.Product.Price <= filterData.PriceRangeEnd )
);


            int TotalCount = products.Count(); //搜尋出來的資料總共有幾筆
            int pageSize = filterData.pageSize ?? 9; //每頁多少筆資料
            int TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize); //計算出總共有幾頁
            int page = filterData.Page ?? 1; //第幾頁

            //取出篩過後書籍的所有分類
            List<ProductDetailsCategoryDto> productsDetailsCategories =
                products.GroupBy(p => p.Product.DetailsCategory.Id)
                        .Select(g => new ProductDetailsCategoryDto
                        {
                            Id = g.First().Product.DetailsCategory.Id,
                            Name = g.First().Product.DetailsCategory.Name
                        })
                        .ToList();

            //取出篩選後的商品的圖片名稱
            var productIds = products.Select(x => x.ProductId).ToList();
            var productPictures = _db.ProductPictures.Where(x => productIds.Contains(x.ProductId)).ToList();
            var productNames = productPictures.Select(x => x.Name).ToList();


            //取出分頁資料
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            var productsDto = products.Select(x => new BookProductDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                BookId = x.BookId,
                PublisherId = x.PublisherId,
                ISBN = x.ISBN,
                ProductName = x.Product.Name,
                ProductCategory = x.Product.Category,
                BookLanguage = x.Book.Language,
                Author = x.Book.Author,
                PublisherName = x.Publisher.Name,
                PublishDate = x.PublishDate,
                ProductStatus = x.Product.ProductStatus,
                Stock = x.Product.Stock,
                Price = x.Product.Price,
                RealPrice = x.Product.RealPrice,
                DiscountDegree = x.Product.DiscountDegree,
                Description = x.Product.Description,
                DetailsCategoryId = x.Product.DetailsCategoryId,
                DetailsCategoryName = x.Product.DetailsCategory.Name

            });

            //設計要回傳的資料格式
            ProductsPagingDto productsPaging = new ProductsPagingDto();
            productsPaging.TotalPages = TotalPages;
            productsPaging.ProductsReslut = productsDto.ToList();
            productsPaging.ProductDetailsCategories = productsDetailsCategories;

            return productsPaging;
        }


        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<BookProductDto> Get(int id)
        {
            var bookProduct = await _db.BookProducts.AsNoTracking()
                .Include(x => x.Book)
                .Include(x => x.Product)
                .Include(x => x.Publisher)
                .Include(x => x.Product.DetailsCategory)
                .Include(x => x.Product.ProductPictures)
                .Include(x => x.Product.ProductKeywords)
                .ThenInclude(pk => pk.Keyword)
                .FirstOrDefaultAsync(x => x.ProductId == id);
            if (bookProduct == null) return null;
            var imageUrl = bookProduct.Product.ProductPictures.Select(x => $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/images/{x.Name}").ToList();

            var productKeywords = bookProduct.Product.ProductKeywords.Select(x => new ProductKeywordDto
            {
                Id = x.Keyword.Id,
                ProductId = x.ProductId,
                KeywordId = x.KeywordId,
                KeywordName = x.Keyword.Name
            }).ToList();

            var bookProductDto = new BookProductDto()
            {
                Id = bookProduct.Id,
                ProductId = bookProduct.ProductId,
                BookId = bookProduct.BookId,
                PublisherId = bookProduct.PublisherId,
                ISBN = bookProduct.ISBN,
                ImageUrl = imageUrl,
                ProductName = bookProduct.Product.Name,
                ProductCategory = bookProduct.Product.Category,
                BookLanguage = bookProduct.Book.Language,
                Author = bookProduct.Book.Author,
                PublisherName = bookProduct.Publisher.Name,
                PublishDate = bookProduct.PublishDate,
                ProductStatus = bookProduct.Product.ProductStatus,
                Stock = bookProduct.Product.Stock,
                Price = bookProduct.Product.Price,
                RealPrice = bookProduct.Product.RealPrice,
                DiscountDegree = bookProduct.Product.DiscountDegree,
                Description = bookProduct.Product.Description,
                DetailsCategoryId = bookProduct.Product.DetailsCategoryId,
                DetailsCategoryName = bookProduct.Product.DetailsCategory.Name,
                ProductKeywords = productKeywords
            };

            return bookProductDto;
        }

        [HttpGet("GetByDetailsCategory/{productId}")]
        public async Task<ActionResult<IEnumerable<BookProductDto>>> GetByDetailsCategoryId(int productId)
        {
            var bookProduct = await _db.BookProducts.AsNoTracking()
                                                .Include(x => x.Product)
                                                .FirstOrDefaultAsync(x => x.ProductId == productId);
            if (bookProduct == null)
            {
                return NotFound();
            }
            var bookProducts = await _db.BookProducts.AsNoTracking()
                .Include(x => x.Book)
                .Include(x => x.Product)
                .Include(x => x.Publisher)
                .Include(x => x.Product.DetailsCategory)
                .Include(x => x.Product.ProductPictures)
                .Include(x => x.Product.ProductKeywords)
                .ThenInclude(pk => pk.Keyword)
                .Where(x => x.Product.DetailsCategoryId == bookProduct.Product.DetailsCategoryId && x.ProductId != productId && x.ISBN != bookProduct.ISBN)
                .OrderBy(x => x.PublishDate).ToListAsync();


            var threeBookProduct = bookProducts.Take(3);

            var bookProductDtos = threeBookProduct.Select(x => new BookProductDto()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                BookId = x.BookId,
                PublisherId = x.PublisherId,
                ISBN = x.ISBN,
                ImageUrl = x.Product.ProductPictures.Select(x => $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/images/{x.Name}").ToList(),
                ProductName = x.Product.Name,
                ProductCategory = x.Product.Category,
                BookLanguage = x.Book.Language,
                Author = x.Book.Author,
                PublisherName = x.Publisher.Name,
                PublishDate = x.PublishDate,
                ProductStatus = x.Product.ProductStatus,
                Stock = x.Product.Stock,
                Price = x.Product.Price,
                RealPrice = x.Product.RealPrice,
                DiscountDegree = x.Product.DiscountDegree,
                Description = x.Product.Description,
                DetailsCategoryId = x.Product.DetailsCategoryId,
                ProductKeywords = x.Product.ProductKeywords.Select(x => new ProductKeywordDto
                {
                    Id = x.Keyword.Id,
                    ProductId = x.ProductId,
                    KeywordId = x.KeywordId,
                    KeywordName = x.Keyword.Name
                }).ToList()
            });
            return Ok(bookProductDtos);
        }

        [HttpGet("GetByPublishDate")]
        public async Task<ActionResult<IEnumerable<BookProductDto>>> GetByPublishDate()
        {

            var bookProducts = await _db.BookProducts.AsNoTracking()
                .Include(x => x.Book)
                .Include(x => x.Product)
                .Include(x => x.Publisher)
                .Include(x => x.Product.DetailsCategory)
                .Include(x => x.Product.ProductPictures)
                .Include(x => x.Product.ProductKeywords)
                .ThenInclude(pk => pk.Keyword)
                .OrderByDescending(x => x.PublishDate).ToListAsync();


            var fourBookProduct = bookProducts.Take(4);

            var bookProductDtos = fourBookProduct.Select(x => new BookProductDto()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                BookId = x.BookId,
                PublisherId = x.PublisherId,
                ISBN = x.ISBN,
                ImageUrl = x.Product.ProductPictures.Select(x => $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/images/{x.Name}").ToList(),
                ProductName = x.Product.Name,
                ProductCategory = x.Product.Category,
                BookLanguage = x.Book.Language,
                Author = x.Book.Author,
                PublisherName = x.Publisher.Name,
                PublishDate = x.PublishDate,
                ProductStatus = x.Product.ProductStatus,
                Stock = x.Product.Stock,
                Price = x.Product.Price,
                RealPrice = x.Product.RealPrice,
                DiscountDegree = x.Product.DiscountDegree,
                Description = x.Product.Description,
                DetailsCategoryId = x.Product.DetailsCategoryId,
                DetailsCategoryName = x.Product.DetailsCategory.Name,
                ProductKeywords = x.Product.ProductKeywords.Select(x => new ProductKeywordDto
                {
                    Id = x.Keyword.Id,
                    ProductId = x.ProductId,
                    KeywordId = x.KeywordId,
                    KeywordName = x.Keyword.Name
                }).ToList()
            });
            return Ok(bookProductDtos);
        }

        [HttpGet("GetByMostProductOrder")]
        public async Task<ActionResult<IEnumerable<BookProductDto>>> GetByMostProductOrder()
        {
            var mostProductbyOrder = await _db.OrderDetails
                                              .GroupBy(x=>x.ProductId)
                                              .OrderByDescending(group => group.Count())
                                              .Take(6)
                                              .Select(x=>x.Key)
                                              .ToListAsync();

            var bookProducts = await _db.BookProducts.AsNoTracking()
                .Include(x => x.Book)
                .Include(x => x.Product)
                .Include(x => x.Publisher)
                .Include(x => x.Product.DetailsCategory)
                .Include(x => x.Product.ProductPictures)
                .Include(x => x.Product.ProductKeywords)
                .ThenInclude(pk => pk.Keyword)
                .Where(x => mostProductbyOrder.Contains(x.ProductId) )  
                .ToListAsync();

            var sixBookProduct = bookProducts.Take(6);

            var bookProductDtos = sixBookProduct.Select(x => new BookProductDto()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                BookId = x.BookId,
                PublisherId = x.PublisherId,
                ISBN = x.ISBN,
                ImageUrl = x.Product.ProductPictures.Select(x => $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/images/{x.Name}").ToList(),
                ProductName = x.Product.Name,
                ProductCategory = x.Product.Category,
                BookLanguage = x.Book.Language,
                Author = x.Book.Author,
                PublisherName = x.Publisher.Name,
                PublishDate = x.PublishDate,
                ProductStatus = x.Product.ProductStatus,
                Stock = x.Product.Stock,
                Price = x.Product.Price,
                RealPrice = x.Product.RealPrice,
                DiscountDegree = x.Product.DiscountDegree,
                Description = x.Product.Description,
                DetailsCategoryId = x.Product.DetailsCategoryId,
                DetailsCategoryName = x.Product.DetailsCategory.Name,
                ProductKeywords = x.Product.ProductKeywords.Select(x => new ProductKeywordDto
                {
                    Id = x.Keyword.Id,
                    ProductId = x.ProductId,
                    KeywordId = x.KeywordId,
                    KeywordName = x.Keyword.Name
                }).ToList()
            });
            return Ok(bookProductDtos);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


    /// <summary>
    /// 商品分類
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ProductsCategoryController : ControllerBase
    {
        private AppDbContext _db;
        public ProductsCategoryController(AppDbContext db)
        {
            _db = db;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var productCategory = _db.Products.AsNoTracking()
                                     .Select(x => x.Category)
                                     .Distinct()
                                     .OrderByDescending(x => x)
                                     .ToList();

            var bookCategory = _db.Books.AsNoTracking()
                                        .Select(x => x.Category)
                                        .Distinct();

            return productCategory;
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
