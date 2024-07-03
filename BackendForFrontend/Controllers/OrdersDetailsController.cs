using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using BackendForFrontend202401.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendForFrontend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersDetailsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public OrdersDetailsController(AppDbContext context)
		{
			_context = context;
		}


		// GET: api/<OrdersDetailsController>
		[HttpGet("{Id}")]
		public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> Get(int Id)
		{			
			//var orderDetailsDto = await _context.Orders.Where(x => x.Id == Id).FirstOrDefaultAsync();

			var orderDetails = await _context.OrderDetails
											.Include(x => x.Product)
											.Where(x => x.OrderId == Id)
											.Select(x => new OrderDetailsDto
                                            {

												OrderId = x.OrderId,
												ProductId = x.ProductId,
												ProductName = x.Product.Name,
												Quantity = x.Quantity,
												UnitPrice = x.UnitPrice,
											})
											.ToListAsync();

			return orderDetails;
		}



		// POST api/<OrdersDetailsController>
		[HttpPost]
		public async Task<ActionResult<string>> Post(OrderDetailsDto orderDetailsDto)
		{
			// 判斷是否購物車細節為空，若是則新增一個商品細節
			if (orderDetailsDto == null)
			{
				return BadRequest("無效的資料");
			}

			try
			{
				var existingOrderDetail = await _context.OrderDetails
										.FirstOrDefaultAsync(x => x.ProductId == orderDetailsDto.ProductId && x.OrderId == orderDetailsDto.OrderId);


				if (existingOrderDetail == null)
				{
					var newOrderDetail = new OrderDetail
					{
						OrderId = (int)orderDetailsDto.OrderId,
						ProductId = orderDetailsDto.ProductId,
						Quantity = orderDetailsDto.Quantity,
						UnitPrice = orderDetailsDto.UnitPrice,
					};
					_context.OrderDetails.Add(newOrderDetail);
					await _context.SaveChangesAsync();

					return Ok("訂單細項新增成功");
				}
				else
				{
					//沒有空的要更新數量
					existingOrderDetail.Quantity = orderDetailsDto.Quantity;

					existingOrderDetail.OrderId = (int)orderDetailsDto.OrderId;

					existingOrderDetail.ProductId = orderDetailsDto.ProductId;
					await _context.SaveChangesAsync();
					return Ok("數量更新成功");
				}
			}
			catch (Exception)
			{
				return "新增失敗";
			}
		}

		// PUT api/<OrdersDetailsController>/5
		[HttpPut("{id}")]
		public async Task<ActionResult<string>> Put(int id, OrderDetail orderDetail)
		{
			if(id != orderDetail.Id) {
				return "更新失敗";
					};
			try
			{
				var existingOrderDetail = await _context.OrderDetails.FindAsync(id);
				//假設購物車裡面沒有想同商品的話
				if (existingOrderDetail == null)
				{
					return NotFound("購物車商品未存在，請使用 POST方法新增購物車");
				}


				//更新數量
				existingOrderDetail.Quantity = orderDetail.Quantity;

				await _context.SaveChangesAsync();
				return Ok("數量更新成功");
			}
			catch (Exception)
			{
				return ("失敗囉廢物");
			}

		}

	}
}
