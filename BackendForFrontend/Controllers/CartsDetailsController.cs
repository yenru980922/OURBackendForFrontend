using BackendForFrontend.Models.EFModels;
using BackendForFrontend202401.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartsDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        //抓取會員Id顯示會員下有甚麼購物車細項
        //3.17測試完成
        public async Task<ActionResult<IEnumerable<CartDetailsDto>>> Get(int Id)
        {
            var cartsDto = await _context.Carts.Where(x => x.MemberId == Id).FirstOrDefaultAsync();
            List<CartDetailsDto> cartsDetail = null;

            if (cartsDto != null)
            {
                cartsDetail = await _context.CartDetails
                                            .Include(x => x.Product)
                                            .Where(x => x.CartId == cartsDto.Id)
                                            .Select(x => new CartDetailsDto
                                            {
                                                Id = x.Id,
                                                CartId = x.CartId,
                                                ProductId = x.ProductId,
                                                ProductName = x.Product.Name,
                                                Quantity = x.Quantity,
                                                UnitPrice = x.UnitPrice,
                                            })
                                            .ToListAsync();
            }
            else
            {
                return NotFound("沒有購物車細項");
            }
            return cartsDetail;
        }


        // POST api/<controller>
        //創立新的
        //3.18測試完成
        [HttpPost]
        public async Task<ActionResult<string>> Post(int memberId, int productId)
        {
            // 判斷是否購物車細節為空，若是則新增一個商品細節
            if (productId == null || memberId == null)
            {
                return BadRequest("無效的資料");
            }

            var existingCart = await _context.Carts
                                        .FirstOrDefaultAsync(x => x.MemberId == memberId);

            if (existingCart == null)
            {
                existingCart = new Cart
                {
                    MemberId = memberId,
                    TotalAmount = 0,
                };
                _context.Carts.Add(existingCart);
                await _context.SaveChangesAsync();
            }

            try
            {
                var existingCartDetail = await _context.CartDetails
                    .FirstOrDefaultAsync(x => x.ProductId == productId && x.CartId == existingCart.Id);

                var product = await _context.Products.FindAsync(productId);

                if (existingCartDetail == null)
                {
                    var newCartDetail = new CartDetail
                    {
                        CartId = existingCart.Id,
                        ProductId = productId,
                        Quantity = 1,
                        UnitPrice = product.Price,
                    };
                    _context.CartDetails.Add(newCartDetail);
                    await _context.SaveChangesAsync();

                    return Ok("購物車細項新增成功");
                }
                else
                {
                    //沒有空的要更新數量
                    existingCartDetail.Quantity++;
                    await _context.SaveChangesAsync();
                    return Ok("數量更新成功");
                }
            }
            catch (Exception)
            {
                return "新增失敗";
            }
        }




        // PUT api/<controller>/5
        //更新
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, int quantity)
        {
            //if (cartDetail == null)
            //{
            //    return BadRequest("沒找到商品餒，你有加過嗎?");
            //}
            try
            {
                var existingCartDetail = await _context.CartDetails.FindAsync(id);
                //假設購物車裡面沒有想同商品的話
                if (existingCartDetail == null)
                {
                    return NotFound("購物車商品未存在，請使用 POST方法新增購物車");
                }


                //更新數量
                existingCartDetail.Quantity = quantity;

                await _context.SaveChangesAsync();
                return Ok("數量更新成功");
            }
            catch (Exception)
            {
                return ("失敗囉廢物");
            }

        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartDetail(int id)
        {
            var cartDetail = await _context.CartDetails.FindAsync(id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            _context.CartDetails.Remove(cartDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
