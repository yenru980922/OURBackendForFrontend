using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookOrderDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookOrderDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<UsedBookOrderDetailDto>> Get(int orderId)
        {
            var orderDetails = await _context.UsedBooksOrderDetails.Where(i => i.OrderID == orderId).Include(b=>b.Book)
                .Select(i => new UsedBookOrderDetailDto
                {
                    Id = i.Id,
                    BookID = i.BookID,
                    BookName=i.Book.Name,
                    UnitPrice = i.UnitPrice
                }).ToListAsync();

            return orderDetails;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(UsedBookOrderDetailDto orderDetail)
        {
            UsedBooksOrderDetail usedBooksOrderDetail = new UsedBooksOrderDetail
            {
                BookID= orderDetail.BookID,
                OrderID= orderDetail.OrderID,
                UnitPrice= orderDetail.UnitPrice
            };

            _context.UsedBooksOrderDetails.Add(usedBooksOrderDetail);
            await _context.SaveChangesAsync();

            return ("新增成功");
        }
    }
}
