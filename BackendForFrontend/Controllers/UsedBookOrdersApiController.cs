using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookOrdersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookOrdersApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(UsedBooksOrder order)
        {
            //todo
            _context.UsedBooksOrders.Add(order);
            await _context.SaveChangesAsync();

            return (order.Id);
        }

        //訂單搜尋
        [HttpGet]
        public async Task<IEnumerable<UsedBookOrderDto>> Get(int memberId, string type)
        {
            if (type == "buyer") 
            { 
            var orders = await _context.UsedBooksOrders.Where(i => i.BuyerId == memberId).Include(s => s.Seller)
                .Select(i => new UsedBookOrderDto
                {
                    Id = i.Id,
                    OrderDate = i.OrderDate,
                    TotalAmount = i.TotalAmount,
                    Status = i.Status,
                    SellerName = i.Seller.Name,
                    ShippingFee = i.ShippingFee,
                }).ToListAsync();
                return orders;
            }
            else if (type == "seller")
            {
                var orders = await _context.UsedBooksOrders.Where(i => i.SellerId == memberId).Include(b => b.Buyer)
                .Select(i => new UsedBookOrderDto
                {
                    Id = i.Id,
                    OrderDate = i.OrderDate,
                    TotalAmount = i.TotalAmount,
                    Status = i.Status,
                    BuyerName = i.Buyer.Name,
                    ShippingFee= i.ShippingFee,
                }).ToListAsync();
                return orders;
            }
            else { return null; }
        }

        //更新訂單狀態
        [HttpPut]
        public async Task<ActionResult<string>> Put(int Id, string status)
        {
            UsedBooksOrder order = _context.UsedBooksOrders.Find(Id);

            if (order == null) { return ("訂單編號不存在"); }

            order.Status = status;

            try
            {
                await _context.SaveChangesAsync();
                return ("修改成功");
            }
            catch (Exception ex)
            {
                // 處理保存失敗的情況
                return StatusCode(500, "無法保存更改: " + ex.Message);
            }
        }



        [HttpGet("bookId/{bookId}")]
        public async Task<ActionResult<string>> GetUsedBookStateByBookId(int bookId)
        {

            var UsedBooksOrders = await _context.UsedBooksOrderDetails
                                           .Include(ub => ub.Order)
                                           .Where(x => x.Id == bookId)
                                           .Select(ub => ub.Order.Status)
                                           .FirstOrDefaultAsync();
            return Ok(UsedBooksOrders);
        }
    }
}
