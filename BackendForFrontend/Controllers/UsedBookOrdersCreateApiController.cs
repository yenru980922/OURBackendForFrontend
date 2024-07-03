using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookOrdersCreateApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookOrdersCreateApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(UsedBookCartsDto[] items, int buyerId, int fee, string method, string paymentNumber, int paymentAmount)
        {
            //找到所有賣家Id
            var sellerIds = items.Select(item => item.SellerID).ToHashSet();

            var orderIds = new List<int>();
            var orderDetails = new List<UsedBooksOrderDetail>();

            foreach (var sellerId in sellerIds)
            {
                //計算訂單金額
                var amount = 0;
                foreach (var item in items)
                {
                    if (item.SellerID == sellerId)
                    {
                        amount += (int)item.UnitPrice;
                    }
                }
                //新建訂單
                var order = new UsedBooksOrder
                {
                    BuyerId = buyerId,
                    SellerId = sellerId,
                    TotalAmount = amount,
                    Status = "未付款",
                    ShippingFee = fee,
                    PaymentMethod = method
                };
                _context.UsedBooksOrders.Add(order);
                await _context.SaveChangesAsync();

                //紀錄訂單編號
                orderIds.Add(order.Id);

                //新建orderdetail
                foreach (var item in items)
                {
                    if (item.SellerID == sellerId)
                    {
                        UsedBooksOrderDetail usedBooksOrderDetail = new UsedBooksOrderDetail
                        {
                            BookID = item.BookID,
                            OrderID = order.Id,
                            UnitPrice = (int)item.UnitPrice
                        };

                        orderDetails.Add(usedBooksOrderDetail);
                    }
                }
            }
            await Task.WhenAll(
                _context.UsedBooksOrderDetails.AddRangeAsync(orderDetails) // 保存订单详情
            ) ;

            //訂單編號轉為字串顯示
            var orderIdString = string.Join(",", orderIds);

            //新增付款紀錄
            UsedBookPaymentRecord data = new UsedBookPaymentRecord
            {
                PaymentNumber = paymentNumber,
                OrderId = orderIdString,
                PaymentAmount = paymentAmount,
                status = false
            };
            _context.UsedBookPaymentRecords.Add(data);
            await _context.SaveChangesAsync();

            return ("新增成功");
        }
    }
}
