using BackendForFrontend.Controllers;
using BackendForFrontend.Models.EFModels;
using BackendForFrontend.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackendForFrontend.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class EcpayPaymentsController : ControllerBase
	{
		private readonly EcpayService _ecpayService;

		private readonly AppDbContext _db;

		public EcpayPaymentsController(EcpayService ecpayService)
		{
			_ecpayService = ecpayService;

		}


		// GET: api/Order/ECPay
		[HttpGet]
		[Route("ECPay")]
		public ActionResult<Object> GetECPayForm([FromQuery] int orderId)
		{
			try
			{
				//string phone = ValidateToken();
				//if (phone == "401") return Unauthorized();

				//var orderDto = _orderService.GetOrder(phone, orderId);

				//if (orderDto.RtnCode == 1 || orderDto.Status == 1) return BadRequest("該訂單已付款");

				//string backEnd = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

				//走實體IP
				//string backEnd = $"https://6249-114-25-133-225.ngrok-free.app";

				//string backEnd = $"https://localhost:7236";
				//string frontEnd = $"localhost:7236";

				var orderDic = _ecpayService.GetECPayDic(orderId);

				return orderDic;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST: api/Order/ECPay
		[HttpPost]

		public ActionResult PostFromECPay([FromForm] IFormCollection col)
		{
			try
			{
				var data = new Dictionary<string, string>();
				foreach (string key in col.Keys)
				{
					data.Add(key, col[key].ToString() ?? "");
				}
				int rtnCode = int.Parse(data["RtnCode"]);
				if(rtnCode != 1)return BadRequest("交易失敗");

				int orderId = _ecpayService.UpdateECpay(data);
				return Redirect($"http://127.0.0.1:5173/ecpay");
				//return Ok()?orderId={orderId};

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}

}

