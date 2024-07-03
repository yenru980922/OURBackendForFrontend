
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Transactions;
using System.Web;
using static NuGet.Packaging.PackagingConstants;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BackendForFrontend.Models.Services
{
	public class EcpayService
	{
		private readonly AppDbContext _db;

		public EcpayService(AppDbContext db)
		{
			_db = db;

		}



		public Dictionary<string, string> GetECPayDic(int orderId)
		{
			var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
			if (order == null) throw new Exception("訂單比對異常");

			//產生綠界交易序號20碼(前10碼訂單後10碼亂數)
			string merchantTradeNo = MTNBulder(order.Id);

			// 編寫字典以利加密(綠界要求)
			var dicOrder = new Dictionary<string, string>
			{
				{ "MerchantID", "3002607" },
				{ "MerchantTradeNo", merchantTradeNo },
				{ "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
				{ "PaymentType", "aio"},
				{ "TotalAmount", order.TotalAmount.ToString() ?? "0" },
				{ "TradeDesc", "Bookstore" },   // todo 交易描述
                { "ItemName",  "Bookstore Order" },
				{ "ReturnURL",  $"https://localhost:7236/api/Ecpay/notpay?orderId={order.Id}"},  // 綠界 POST 回後端這個傳出去接不到(因為這裡要傳實體IP)
                { "ChoosePayment", "ALL" },
				{ "EncryptType",  "1" },
               /* { "ClientBackURL", $"{frontEnd}" }, */   // 僅為綠界端轉址返回按鈕，並無 POST 功能非必填
                { "OrderResultURL", $"https://localhost:7236/api/EcpayPayments" },   // 綠界 POST 回後端，唯一的post
            };

			// 透過字典(綠界要求)
			dicOrder["CheckMacValue"] = GetCheckMacValue(dicOrder);


			// 填入資料庫以利檢查



			//我需要知道綠界傳回來的交易是否正確
			order.MerchantTradeNo = merchantTradeNo;


			//這是綠界反傳回來的資料我不需要(如果需要要多新增綠界資料夾)
			//order.TradeNo = merchantTradeNo.Substring(10, 10);
			//order.TradeDate = dicOrder["MerchantTradeDate"];
			//order.SimulatePaid = 1;
			//order.CheckMacValue = dicOrder["CheckMacValue"];

			_db.SaveChanges();

			return dicOrder;
		}

		public int UpdateECpay(Dictionary<string, string> dictionary)
		{
			var checkvalue = GetCheckMacValue(dictionary);
			var checkMacValue = dictionary["CheckMacValue"];

			if (checkvalue != checkMacValue) throw new Exception("校驗錯誤");

			var order = _db.Orders.Where(o => o.MerchantTradeNo == dictionary["MerchantTradeNo"]).FirstOrDefault();

			if (order == null) throw new Exception("查無該筆訂單");

			int rtnCode = int.Parse(dictionary["RtnCode"]);


			if (rtnCode == 1)
			{

				order.Status = "待出貨";
				order.OrderDate = Convert.ToDateTime(dictionary["PaymentDate"]);
				order.PaymentMethod = "信用卡";

			}

			_db.SaveChanges();

			return order.Id;
		}

		public string GetCheckMacValue(Dictionary<string, string> dictionary)
		{
			string merchantKey = "pwFHCqoQZGmho4w6";
			string merchantIv = "EkRm7iFT261dpevs";

			string raw = $"HashKey={merchantKey}&" +
						 string.Join("&", dictionary.Where(x => x.Key != "CheckMacValue")
													.OrderBy(x => x.Key)
													.Select(p => $"{p.Key}={p.Value}")) +
						 $"&HashIV={merchantIv}";

			string encoded = HttpUtility.UrlEncode(raw).ToLower();

			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encoded));
				string checkMacValue = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
				return checkMacValue;
			}
		}

		// 產生綠界交易碼
		private string MTNBulder(int orderId)
		{
			return $"{new string('0', 10 - orderId.ToString().Length)}{orderId}{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10)}";
		}

		//private string GetCheckMacValue(Dictionary<string, string> dicOrder)
		//{
		//    var param = dicOrder.Keys.OrderBy(x => x).Select(key => key + "=" + dicOrder[key]).ToList();
		//    string checkValue = string.Join("&", param);

		//    //綠界提供測試用的 HashKey 真實註冊會有另一組
		//    var hashKey = "pwFHCqoQZGmho4w6";
		//    //綠界提供測試用的 HashIV 真實註冊會有另一組
		//    var hashIV = "EkRm7iFT261dpevs";

		//    checkValue = $"HashKey={hashKey}&" + checkValue + $"&HashIV={hashIV}";
		//    checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
		//    checkValue = GetSHA256(checkValue);
		//    checkValue = checkValue.ToUpper();

		//    return checkValue;
		//}

		//private string GetSHA256(string value)
		//{
		//    var result = new StringBuilder();
		//    var sha256 = SHA256.Create();
		//    var bts = Encoding.UTF8.GetBytes(value);
		//    var hash = sha256.ComputeHash(bts);

		//    for (int i = 0; i < hash.Length; i++)
		//    {
		//        result.Append(hash[i].ToString("X2"));
		//    }

		//    return result.ToString();
		//}
	}
}
