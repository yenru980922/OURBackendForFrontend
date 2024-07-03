using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
	public class UsedBookOrderDetailDto
	{
		public int Id { get; set; }

		public int OrderID { get; set; }

		public int BookID { get; set; }

		public string BookName { get; set; }

		public int UnitPrice { get; set; }

		//public string BuyerName { get; set; }

		//public string SellerName { get; set; }

		//public DateTime OrderDate { get; set; }

		//public int TotalAmount { get; set; }

		//public string Status { get; set; }

		//public int ShippingFee { get; set; }
	}
}