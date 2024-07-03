//using BackendForFrontend.Models.EFModels;
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using UseECPay.DTO;
//using UseECPay.Models;

//namespace UseECPay.Controllers
//{
//	public class HomeController : Controller
//	{
//		private readonly ILogger<HomeController> _logger;
//		ECPayContext _context;
//		public HomeController(ILogger<HomeController> logger, ECPayContext context)
//		{
//			_logger = logger;
//			_context = context;
//		}

//		private async Task<string> AddOrder(ECPayDTO EcPayDto)
//		{
//			EcpayOrders Orders = new EcpayOrders();
//			Orders.MemberId = EcPayDto.MerchantID;
//			Orders.MerchantTradeNo = EcPayDto.MerchantTradeNo;
//			Orders.RtnCode = 0; //未付款
//			Orders.RtnMsg = "訂單成功尚未付款";
//			Orders.TradeNo = EcPayDto.MerchantID.ToString();
//			Orders.TradeAmt = EcPayDto.TotalAmount;
//			Orders.PaymentDate = Convert.ToDateTime(EcPayDto.MerchantTradeDate);
//			Orders.PaymentType = EcPayDto.PaymentType;
//			Orders.PaymentTypeChargeFee = "0";
//			Orders.TradeDate = EcPayDto.MerchantTradeDate;
//			Orders.SimulatePaid = 0;
//			_context.EcpayOrders.Add(Orders);
//			await _context.SaveChangesAsync();
//			return "OK";
//		}

//		public async Task<ActionResult> Index()
//		{
//			ECPayDTO Ec = new ECPayDTO
//			{
//				MerchantID = "3002607",
//				MerchantTradeNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20),
//				MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
//				PaymentType = "aio",
//				TotalAmount = 100,
//				TradeDesc = "無",
//				ItemName = "測試商品",
//			};
//			//Add Order
//			await AddOrder(Ec);

//			//Do Payment
//			var website = $"https://localhost:7147";            //需填入你的網址
//			var order = new Dictionary<string, string>
//			{
//				{ "MerchantID", Ec.MerchantID },
//				{ "MerchantTradeNo",  Ec.MerchantTradeNo},
//				{ "MerchantTradeDate",  Ec.MerchantTradeDate},
//				{ "PaymentType",  Ec.PaymentType},
//				{ "TotalAmount",  Convert.ToString(Ec.TotalAmount)},
//				{ "TradeDesc", Ec.TradeDesc },
//				{ "ItemName",  Ec.ItemName},
//				{ "ExpireDate",  "3"},
//				{ "ReturnURL",  $"{website}/api/ECPay/AddPayInfo"},
//				{ "OrderResultURL", $"{website}/Home/PayInfo/{Ec.MerchantTradeNo}"},
//				{ "ChoosePayment", "ALL"},
//				{ "PaymentInfoURL",  $"{website}/api/ECPay/AddAccountInfo"},
//				{ "EncryptType",  "1"},
//				{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{Ec.MerchantTradeNo}"},
//			};
//			//檢查碼
//			order["CheckMacValue"] = GetCheckMacValue(order);
//			return View(order);
//		}

//		//如果是Web API Controller, 且需要停用CORS
//		//[HttpPost("PayInfo/{id}"]
//		//public async Task<ActionResult> PayInfo([FromForm]IFormCollection col)

//		[HttpPost]
//		public async Task<ActionResult> PayInfo(IFormCollection col)
//		{
//			var data = new Dictionary<string, string>();
//			foreach (string key in col.Keys)
//			{
//				data.Add(key, col[key]);
//			}
//			var Orders = _context.EcpayOrders.ToList().Where(m => m.MerchantTradeNo == col["MerchantTradeNo"]).FirstOrDefault();
//			Orders.RtnCode = int.Parse(col["RtnCode"]);
//			if (col["RtnMsg"] == "Succeeded")
//			{
//				Orders.RtnMsg = "已付款";
//				Orders.PaymentDate = Convert.ToDateTime(col["PaymentDate"]);
//				Orders.SimulatePaid = int.Parse(col["SimulatePaid"]);
//				await _context.SaveChangesAsync();
//			}
//			return View("ECpayView", data);
//		}
//		/// step5 : 取得虛擬帳號 資訊  ClientRedirectURL
//		[HttpPost]
//		public async Task<ActionResult> AccountInfo(IFormCollection col)
//		{
//			var data = new Dictionary<string, string>();
//			foreach (string key in col.Keys)
//			{
//				data.Add(key, col[key]);
//			}
//			var Orders = _context.EcpayOrders.ToList().Where(m => m.MerchantTradeNo == col["MerchantTradeNo"]).FirstOrDefault();
//			Orders.RtnCode = int.Parse(col["RtnCode"]);
//			if (col["RtnMsg"] == "Succeeded")
//			{
//				Orders.RtnMsg = "已付款";
//				Orders.PaymentDate = Convert.ToDateTime(col["PaymentDate"]);
//				Orders.SimulatePaid = int.Parse(col["SimulatePaid"]);
//				await _context.SaveChangesAsync();
//			}
//			return View("ECpayView", data);
//		}
//		private string GetCheckMacValue(Dictionary<string, string> order)
//		{
//			var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
//			string checkValue = string.Join("&", param);
//			//測試用的 HashKey
//			var hashKey = "pwFHCqoQZGmho4w6";
//			//測試用的 HashIV
//			var HashIV = "EkRm7iFT261dpevs";
//			checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
//			checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
//			checkValue = GetSHA256(checkValue);
//			return checkValue.ToUpper();
//		}
//		private string GetSHA256(string value)
//		{
//			var result = new StringBuilder();
//			var sha256 = SHA256.Create();
//			var bts = Encoding.UTF8.GetBytes(value);
//			var hash = sha256.ComputeHash(bts);

//			for (int i = 0; i < hash.Length; i++)
//			{
//				result.Append(hash[i].ToString("X2"));
//			}

//			return result.ToString();
//		}

//		public IActionResult Privacy()
//		{
//			return View();
//		}

//		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//		public IActionResult Error()
//		{
//			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//		}
//	}
//}