using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class UsedBookOrderDto
    {
        public int Id { get; set; }

        public int BuyerId { get; set; }

		public string BuyerName { get; set; }

		public int SellerId { get; set; }

		public string SellerName { get; set; }

		public DateTime OrderDate { get; set; }

        public int TotalAmount { get; set; }

        public string Status { get; set; }

        public int ShippingFee { get; set; }

        public string PaymentMethod { get; set; }
    }
}