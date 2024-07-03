using Microsoft.AspNetCore.Mvc;
using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.Interfaces;
using BackendForFrontend.Models.Services;
using BackendForFrontend.Models.EFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;



namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBooksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IGoogleBooksService _googleBooksService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsedBooksController(IWebHostEnvironment hostingEnvironment, AppDbContext db, IGoogleBooksService googleBooksService)
        {
            _db = db;
            _googleBooksService = googleBooksService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<UsedBookDto>> GetBookByISBN(string isbn)
        {
            var bookInfo = await _googleBooksService.GetBookByISBNAsync(isbn);
            if (bookInfo != null)
            {
                return Ok(bookInfo);
            }
            else
            {
                return NotFound();
            }
        }

        //取得單本書
        [HttpGet("Id/{Id}")]
        public async Task<ActionResult<UsedBookDto>> GetBookById(int Id)
        {
            var usedBook = await _db.UsedBooks
                .Include(ub => ub.Category) // 確保包括相關的類別信息
                .FirstOrDefaultAsync(ub => ub.Id == Id); // 使用 Id 參數來獲取特定的書籍
            if (usedBook == null)
            {
                return NotFound($"沒有找到 ID 為 {Id} 的書籍資料");
            }
            var bookDto = new UsedBookDto
            {
                Id = usedBook.Id,
                MemberId = usedBook.MemberId,
                BookName = usedBook.Name,
                CategoryId = usedBook.CategoryId,
                CategoryName = usedBook.Category?.Name,
                Authors = usedBook.Authors?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                PublisherName = usedBook.PublisherName,
                PublishDate = usedBook.PublishDate,
                ProductStatus = usedBook.ProductStatus,
                Price = usedBook.Price,
                Description = usedBook.Description,
                ISBN = usedBook.ISBN,
                Picture = Convert.ToBase64String(usedBook.Picture),
                BookStatus = usedBook.BookStatus,
                ReleaseDate = usedBook.ReleaseDate,
                ImageLinks = new ImageLinksDto
                {
                    SmallThumbnailPicture = usedBook.SmallThumbnailPicture,
                    ThumbnailPicture = usedBook.ThumbnailPicture,
                    SmallPicture = usedBook.SmallPicture,
                },
                WebReaderLink = usedBook.WebReaderLink
            };

            return Ok(bookDto);
        }

        [HttpGet("Category/Book/{Id}")]
        public async Task<ActionResult<IEnumerable<UsedBookDto>>> GetBookByCategoryAndId(int Id)
        {
            var selfUsedBook = await _db.UsedBooks.FirstOrDefaultAsync(x => x.Id == Id);
            if (selfUsedBook == null) return NotFound("沒有此書籍");
            var usedBook = await _db.UsedBooks
                .AsNoTracking()
                .Include(ub => ub.Category) // 確保包括相關的類別信息
                .Where(ub => ub.Id != Id && ub.CategoryId == selfUsedBook.CategoryId)
                .ToListAsync(); // 使用 Id 和 CategoryId 來獲取特定的書籍

            if (usedBook == null)
            {
                return NotFound($"沒有找到屬於類別 ID 為 {selfUsedBook.CategoryId} 的書籍且書籍 ID 為 {Id} 的資料");
            }

            var usedBookDto = usedBook.Select(x => new UsedBookDto()
            {
                Id = x.Id,
                MemberId = x.MemberId,
                BookName = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.Category?.Name,
                Authors = x.Authors?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                PublisherName = x.PublisherName,
                PublishDate = x.PublishDate,
                ProductStatus = x.ProductStatus,
                Price = x.Price,
                Description = x.Description,
                ISBN = x.ISBN,
                Picture = Convert.ToBase64String(x.Picture),
                BookStatus = x.BookStatus,
                ReleaseDate = x.ReleaseDate,
                ImageLinks = new ImageLinksDto
                {
                    SmallThumbnailPicture = x.SmallThumbnailPicture,
                    ThumbnailPicture = x.ThumbnailPicture,
                    SmallPicture = x.SmallPicture,
                },
                WebReaderLink = x.WebReaderLink
            });

            return Ok(usedBookDto);
        }


        //抓取所有書
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsedBookDto>>> GetAllBooks()
        {
            var allBooks = await _db.UsedBooks
                .ToListAsync();
            if (allBooks == null)
            {
                return NotFound($"沒有任何上架書籍資料");
            }
            var bookDtos = allBooks.Select(ub => new UsedBookDto
            {
                Id = ub.Id,
                MemberId = ub.MemberId,
                BookName = ub.Name,
                CategoryId = ub.CategoryId,
                CategoryName = ub.Category?.Name,
                Authors = ub.Authors?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                PublisherName = ub.PublisherName,
                PublishDate = ub.PublishDate,
                ProductStatus = ub.ProductStatus,
                Price = ub.Price,
                Description = ub.Description,
                ISBN = ub.ISBN,
                Picture = Convert.ToBase64String(ub.Picture),
                BookStatus = ub.BookStatus,
                ReleaseDate = ub.ReleaseDate,
                ImageLinks = new ImageLinksDto
                {
                    SmallThumbnailPicture = ub.SmallThumbnailPicture,
                    ThumbnailPicture = ub.ThumbnailPicture,
                    SmallPicture = ub.SmallPicture,
                },
                WebReaderLink = ub.WebReaderLink

            }).OrderByDescending(x=>x.ReleaseDate).ToList();

            return Ok(bookDtos);
        }


        [HttpGet("userId/{userId}")]
        public async Task<ActionResult<IEnumerable<UsedBookDto>>> GetUserBooks(int userId)
        {
            var userBooks = await _db.UsedBooks
                .Where(ub => ub.MemberId == userId && !ub.IsDeleted)
                .ToListAsync();
            if (userBooks == null)
            {
                return NotFound($"沒有任何上架書籍資料");
            }
            var bookDtos = userBooks.Select(ub => new UsedBookDto
            {
                Id = ub.Id,
                MemberId = ub.MemberId,
                BookName = ub.Name,
                CategoryId = ub.CategoryId,
                CategoryName = ub.Category?.Name, // 如果Category是null，這裡不會拋出異常
                Authors = ub.Authors?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                // 如果Authors是null，Split將不會被調用，將返回一個空的字符串列表
                PublisherName = ub.PublisherName,
                PublishDate = ub.PublishDate,
                ProductStatus = ub.ProductStatus,
                Price = ub.Price,
                Description = ub.Description,
                ISBN = ub.ISBN,
                Picture = Convert.ToBase64String(ub.Picture),
                BookStatus = ub.BookStatus,
                ReleaseDate = ub.ReleaseDate,
                ImageLinks = new ImageLinksDto { SmallThumbnailPicture = ub.SmallThumbnailPicture,
                    ThumbnailPicture = ub.ThumbnailPicture,
                    SmallPicture = ub.SmallPicture,
                },
                WebReaderLink = ub.WebReaderLink

            }).OrderByDescending(x => x.ReleaseDate).ToList();

            return Ok(bookDtos);
        }


        [HttpPost]
        public async Task<ActionResult<string>> Post([FromForm] UsedBookCreatDto usedBookDto)
        {
            if (usedBookDto == null)
            {
                return BadRequest("UsedBookDto object is null");
            }

            // 從 Google Books API 獲取書籍資訊
            var bookInfo = await _googleBooksService.GetBookByISBNAsync(usedBookDto.ISBN);
            if (bookInfo == null)
            {
                return NotFound($"No book found with ISBN {usedBookDto.ISBN}");
            }

            
            try {
                var file = usedBookDto.ImageFile;
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

             

            // 創建新的 UsedBook 
            var newUsedBook = new UsedBook
            {
                Name = bookInfo.BookName,
                Authors = String.Join(",",bookInfo.Authors),
                CategoryId = usedBookDto.CategoryId,                
                PublisherName = bookInfo.PublisherName,
                PublishDate = bookInfo.PublishDate,
                ProductStatus = true,
                Price =usedBookDto.Price,
                Description = bookInfo.Description,
                ISBN =usedBookDto.ISBN,
                Picture = fileBytes,
                BookStatus = usedBookDto.BookStatus,
                ReleaseDate = DateTime.Now,
                MemberId = 2 ,//(int)usedBookDto.MemberId,
                SmallThumbnailPicture = bookInfo.ImageLinks?.SmallThumbnailPicture,
                SmallPicture = bookInfo.ImageLinks?.SmallPicture,
                ThumbnailPicture = bookInfo.ImageLinks?.ThumbnailPicture,
                WebReaderLink= bookInfo.WebReaderLink
            };
            
          
                _db.UsedBooks.Add(newUsedBook);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("無法上架書籍。");
            }

            //// 將 Google Books API 的結果反到DTO
            //usedBookDto.Id = newUsedBook.Id;
            //usedBookDto.BookName = bookInfo.Title; 
            //usedBookDto.Authors = bookInfo.Authors; 
            //usedBookDto.PublisherName = bookInfo.PublisherName; 
            //usedBookDto.PublishDate = bookInfo.PublishDate; 
            //usedBookDto.ImageLinks = bookInfo.ImageLinks;
            //usedBookDto.WebReaderLink= bookInfo.WebReaderLink;

            //return CreatedAtAction(nameof(GetBookByISBN), new { isbn = usedBookDto.ISBN }, usedBookDto);
            return Ok("新增成功");
        }


        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromForm] UsedBookUpdateDto updateDto)
        //{
        //    if (updateDto == null)
        //    {
        //        return BadRequest("Invalid input data");
        //    }

        //    var bookToUpdate = await _db.UsedBooks.FindAsync(id);
        //    if (bookToUpdate == null)
        //    {
        //        return NotFound($"Could not find book with ID {id}");
        //    }

        //    // 圖片更新
        //    if (updateDto.ImageFile != null && updateDto.ImageFile.Length > 0)
        //    {

        //        string uniqueFileName = Guid.NewGuid().ToString() + "_" + updateDto.ImageFile.FileName;
        //        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        // 保存新圖片
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await updateDto.ImageFile.CopyToAsync(fileStream);
        //        }

        //        // 刪除舊圖片
        //        if (!string.IsNullOrWhiteSpace(bookToUpdate.Picture))
        //        {
        //            string oldImagePath = Path.Combine(uploadsFolder, bookToUpdate.Picture);
        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }
        //        }

        //        // 更新圖片路徑
        //        bookToUpdate.Picture = uniqueFileName;
        //    }

        //    // 更新價格
        //    bookToUpdate.Price = updateDto.Price;

        //    _db.UsedBooks.Update(bookToUpdate);
        //    await _db.SaveChangesAsync();

        //    return NoContent(); // 204 No Content
        //}
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateProductStateByBookId(int bookId, [FromBody] bool productState)
        {
            try
            {
                var usedbook = await _db.UsedBooks.FindAsync(bookId);
                usedbook.ProductStatus = productState;
                _db.SaveChangesAsync();
            }
            catch {
                return BadRequest("更新失敗");
            }
            return Content("新增成功");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBook(int id, [FromForm] UsedBookUpdateDto updateDto)
        {
            if (updateDto == null)
                return BadRequest("Invalid input data");

            var bookToUpdate = await _db.UsedBooks.FindAsync(id);
            if (bookToUpdate == null)
            {
                return NotFound($"Could not find book with ID {id}");
            }

            // 如果 updateDto 中有提供 Price，就更新它
            // 然後設置一個標記表示實體已經被更新
            bool isUpdated = false;

            // 圖片更新
            if (updateDto.ImageFile != null && updateDto.ImageFile.Length > 0)
            {
                var file = updateDto.ImageFile;
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
                bookToUpdate.Picture = fileBytes;
                isUpdated = true;
            }

            /// 更新價格
            if (updateDto.Price != 0)
            {
                bookToUpdate.Price = (decimal)updateDto.Price;
                isUpdated = true;
            }

            

            // 檢查是否需要保存更新
            if (isUpdated)
            {
                _db.UsedBooks.Update(bookToUpdate);
                await _db.SaveChangesAsync();
            }

            return NoContent(); // 204 No Content

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookToDelete = await _db.UsedBooks.FindAsync(id);
            if (bookToDelete == null)
            {
                return NotFound($"找不到符合的書籍 {id}");
            }

            // 不是真的刪除，而是將 IsDeleted 標記為 true
            bookToDelete.IsDeleted = true;
            await _db.SaveChangesAsync();

            return NoContent(); // 回傳 204 No Content 狀態碼
        }


    }
}
