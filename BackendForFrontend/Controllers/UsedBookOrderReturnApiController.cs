using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookOrderReturnApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookOrderReturnApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(int orderId, int memberId, string reason)
        {
            UsedBooksReturn data = new UsedBooksReturn
            {
                OrderID = orderId,
                MemberID = memberId,
                ReturnReason = reason,
                Status = "取消申請中"
            };
            _context.UsedBooksReturns.Add(data);

            UsedBooksOrder order = _context.UsedBooksOrders.Find(data.OrderID);
            order.Status = "取消申請中";

            await _context.SaveChangesAsync();

            return ("Ok");
        }
    }
}
