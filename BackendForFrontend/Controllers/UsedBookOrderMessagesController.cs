using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookOrderMessagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookOrderMessagesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<UsedBookOrderMessage>> Get(int orderId)
        {
            var messages = await _context.UsedBookOrderMessages.Where(i => i.OrderId == orderId)
                .Select(i => new UsedBookOrderMessage
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    Message = i.Message,
                    MemberId = i.MemberId,
                    DateTime = i.DateTime
                }).ToListAsync();

            return messages;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(UsedBookOrderMessage message)
        {
            _context.UsedBookOrderMessages.Add(message);
            await _context.SaveChangesAsync();

            return ("新增成功");
        }
    }
}
