using BackendForFrontend.Models.EFModels;
using BackendForFrontend202401.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{memberId}")]
        //抓取
        //3.17測試完成
        public async Task<ActionResult<CartsDto>> Get(int memberId)
        {
            var memebrCart = await _context.Carts.Include(x => x.Member).FirstOrDefaultAsync(x => x.MemberId == memberId);

            CartsDto cartDto = null;
            if (memebrCart != null)
            {
                cartDto = new CartsDto
                {
                    Id = memebrCart.Id,
                    MemberId = memebrCart.MemberId,
                    MemberName = memebrCart.Member.Name,
                    TotalAmount = memebrCart.TotalAmount,
                    DiscountAmount = memebrCart.DiscountAmount,
                    Message = memebrCart.Message,
                    Phone = memebrCart.Phone,
                    Address = memebrCart.Address,

                };
            }
            return cartDto ?? new CartsDto() { Id = 0, MemberId = 0, PaymentMethod = "沒錢", TotalAmount = 0, DiscountAmount = 0, Message = "空" };
        }


        // POST api/<controller>
        //創立
        //3.17測試完成
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] CartsDto cartsDto)
        {
            if (cartsDto == null)
            {
                return BadRequest("介面出錯請稍後");
            }

            try
            {
                var existingCart = await _context.Carts
                                        .FirstOrDefaultAsync(x => x.MemberId == cartsDto.MemberId);

                if (existingCart == null)
                {
                    var newCart = new Cart
                    {
                        MemberId = cartsDto.MemberId,

                    };
                    _context.Carts.Add(newCart);
                    await _context.SaveChangesAsync();

                    return Ok("新增成功");
                }
                else
                {
                    return BadRequest("購物車已存在，請使用 PUT 方法更新購物車");
                }
            }
            catch (Exception)
            {
                return BadRequest("新增失敗");
            }
        }

        // PUT api/<controller>/5
        //更新總金額
        [HttpPut("/totalAmount/{id}")]
        public async Task<ActionResult<string>> PutAmount(int id, int totalAmount)
        {
            if (totalAmount == null)
            {
                return BadRequest("購物車未存在，請使用 POST方法新增購物車");
            }

            var existingCart = await _context.Carts.FindAsync(id);
            //假設購物車裡面沒有相同商品的話
            if (existingCart == null)
            {
                return NotFound("找不到對應的購物車");
            }

            try
            {
                existingCart.TotalAmount = totalAmount;

                _context.Carts.Update(existingCart);
                await _context.SaveChangesAsync();

                return Ok("購物車內容已更新");
            }
            catch (Exception)
            {
                return BadRequest("更新失敗");
            }
        }


        // PUT api/<controller>/5
        //更新
        //3.17測試完成
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, Cart cart)
        {
            if (cart == null)
            {
                return BadRequest("購物車未存在，請使用 POST方法新增購物車");
            }

            var existingCart = await _context.Carts.FindAsync(id);
            //假設購物車裡面沒有相同商品的話
            if (existingCart == null)
            {
                return NotFound("找不到對應的購物車");
            }

            try
            {
                existingCart.TotalAmount = cart.TotalAmount;
                // 更新折扣金額
                existingCart.DiscountAmount = cart.DiscountAmount;
                // 更新備註信息
                existingCart.Message = cart.Message;

                _context.Carts.Update(existingCart);
                await _context.SaveChangesAsync();

                return Ok("購物車內容已更新");
            }
            catch (Exception)
            {
                return BadRequest("更新失敗");
            }
        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            try
            {
                var cartToDelete = await _context.Carts.FindAsync(id);

                if (cartToDelete == null)
                {
                    return NotFound(); // 購物車不存在，回傳 NotFound 結果
                }

                _context.Carts.Remove(cartToDelete);
                await _context.SaveChangesAsync();

                return Ok("購物車刪除成功"); // 回傳成功訊息給用戶
            }
            catch (Exception)
            {
                return BadRequest("刪除失敗"); // 回傳刪除失敗訊息給用戶
            }
        }

    }
}
