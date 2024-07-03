using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using BackendForFrontend.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<OrdersDto>> GetByOrderId(int orderId)
        {
            var orderdataId = await _context.Orders
            .FirstOrDefaultAsync(x => x.Id == orderId);
            if (orderdataId == null)
            {
                return NotFound();
            }
            var ordersDto = new OrdersDto
            {
                Id = orderdataId.Id,
                MemberId = orderdataId.MemberId,
                OrderDate = orderdataId.OrderDate,
                PaymentMethod = orderdataId.PaymentMethod,
                TotalAmount = orderdataId.TotalAmount,
                DiscountAmount = orderdataId.DiscountAmount,
                Status = orderdataId.Status,
                Message = orderdataId.Message,
                Phone = orderdataId.Phone
            };

            return Ok(ordersDto);

        }



        [HttpGet("{memberId}")]
        public async Task<ActionResult<IEnumerable<OrdersDto>>> Get(int memberId)
        {
            var memberOrder = await _context.Orders
                .Include(x => x.Member)
                .Where(x => x.MemberId == memberId)
                .ToListAsync();

            if (memberOrder.Any())
            {
                var ordersDto = memberOrder.Select(order => new OrdersDto
                {
                    Id = order.Id,
                    MemberId = order.MemberId,
                    MemberName = order.Member.Name,
                    OrderDate = order.OrderDate,
                    PaymentMethod = order.PaymentMethod,
                    TotalAmount = order.TotalAmount,
                    DiscountAmount = order.DiscountAmount,
                    Status = order.Status,
                    Message = order.Message,
                    Phone = order.Phone
                });

                return Ok(ordersDto);
            }

            return NotFound(); // 如果找不到相應的訂單，返回 404 Not Found
        }



        // POST api/<OrderApiController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] OrderCreationDto orderCreationDto)
        {
            if (orderCreationDto == null || orderCreationDto.OrdersDto == null || orderCreationDto.OrderDetailsDto == null)
            {
                return BadRequest();
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 創建新訂單
                    var newOrder = new Order
                    {
                        MemberId = orderCreationDto.OrdersDto.MemberId,
                        PaymentMethod = orderCreationDto.OrdersDto.PaymentMethod,
                        OrderDate = orderCreationDto.OrdersDto.OrderDate,
                        TotalAmount = orderCreationDto.OrdersDto.TotalAmount,
                        Status = orderCreationDto.OrdersDto.Status,
                        DiscountAmount = orderCreationDto.OrdersDto.DiscountAmount,
                        Message = orderCreationDto.OrdersDto.Message,
                        Address = orderCreationDto.OrdersDto.Address,
                        Phone = orderCreationDto.OrdersDto.Phone

                    };

                    _context.Orders.Add(newOrder);
                    await _context.SaveChangesAsync();
                    // 新增訂單明細
                    var newOrderDetails = new List<OrderDetail>();
                    foreach (var detailDto in orderCreationDto.OrderDetailsDto)
                    {
                        if (detailDto.ProductId == 0 || detailDto.Quantity == 0 || detailDto.UnitPrice == 0)
                        {
                            return BadRequest();
                        }
                        var newOrderDetail = new OrderDetail
                        {
                            OrderId = newOrder.Id,
                            ProductId = (int)detailDto.ProductId,
                            Quantity = (int)detailDto.Quantity,
                            UnitPrice = (int)detailDto.UnitPrice,
                        };
                        newOrderDetails.Add(newOrderDetail);
                    }

                    // 新增訂單明細到資料庫
                    _context.OrderDetails.AddRange(newOrderDetails);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return Ok(newOrder.Id);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return BadRequest();
                }
            }
        }



        // PUT api/<OrderApiController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, Order order)
        {
            if (order == null)
            {
                return BadRequest("訂單未存在，請使用 POST方法新增訂單");
            }

            var existingOrder = await _context.Orders.FindAsync(id);
            //假設購物車裡面沒有相同商品的話
            if (existingOrder == null)
            {
                return NotFound("找不到對應的訂單");
            }

            try
            {
                // 更新付款方式
                if (order.PaymentMethod != null)
                    existingOrder.PaymentMethod = order.PaymentMethod;

                if (order.TotalAmount != 0)
                    existingOrder.TotalAmount = order.TotalAmount;

                // 更新折扣金額
                if (order.DiscountAmount != 0)
                    existingOrder.DiscountAmount = order.DiscountAmount;
                // 更新備註信息
                if (order.Message != null)

                    existingOrder.Message = order.Message;
                // 更新訂單狀態
                if (order.Status != null)
                    existingOrder.Status = order.Status;

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                return Ok("訂單內容已更新");
            }
            catch (Exception)
            {
                return BadRequest("訂單更新失敗");
            }
        }

    }
    public class OrderService
    {
        private readonly ILineMessagingService _lineMessagingService;

        public OrderService(ILineMessagingService lineMessagingService)
        {
            _lineMessagingService = lineMessagingService;
        }

        public async Task ProcessOrderAsync(Order order)
        {
            // 訂單處理邏輯...

            // 訂單成功後發送LINE通知
            await _lineMessagingService.SendPushMessageAsync(order.Id.ToString(), "您的訂單已成功下訂！");
        }
    }

}
