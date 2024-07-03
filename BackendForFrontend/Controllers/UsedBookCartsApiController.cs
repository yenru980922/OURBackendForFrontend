using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookCartsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookCartsApiController(AppDbContext context)
        {
            _context = context;
        }

        //搜尋會員購物車商品
        [HttpGet]
        public async Task<IEnumerable<UsedBookCartsDto>> Get(int memberId)
        {
            var cartItem = await _context.UsedBooksCarts.Where(i => i.MemberID == memberId).Include(b => b.Book)
                .Select(i => new UsedBookCartsDto
                {
                    Id = i.Id,
                    BookID = i.BookID,
                    Name = i.Book.Name,
                    Picture = Convert.ToBase64String(i.Book.Picture),
                    ProductStatus = i.Book.ProductStatus,
                    BookStatus = i.Book.BookStatus,
                    UnitPrice = i.Book.Price,
                    SellerName = i.Book.Member.Name,
                    SellerID=i.Book.Member.Id
                }).ToListAsync();

            return cartItem;
        }

        //刪除 api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _context.UsedBooksCarts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.UsedBooksCarts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] AddUsedBookToCartDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartItem = await _context.UsedBooksCarts
                .FirstOrDefaultAsync(c => c.MemberID == item.MemberID && c.BookID == item.BookID);
            if (cartItem != null)
            {
                return BadRequest("商品已在購物車中");
            }

            var newCartItem = new UsedBooksCart
            {
                MemberID = item.MemberID,
                BookID = item.BookID,
                AddToCartDate = DateTime.Now
            };

            _context.UsedBooksCarts.Add(newCartItem);
            await _context.SaveChangesAsync();

            return Ok("新增成功");
        }

    }
}
