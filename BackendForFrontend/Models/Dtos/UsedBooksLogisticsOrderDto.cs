using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
	public class UsedBooksLogisticsOrderDto
	{
		public int Id { get; set; }

		public int OrderID { get; set; }

		public string LogisticsCompany { get; set; }

		public string TrackingNumber { get; set; }

		public DateTime EstimateDeliveryDate { get; set; }

		public DateTime? ActualDeliveryDate { get; set; }

		public string PickupMethod { get; set; }

		public string SenderAddress { get; set; }

		public string SenderPhone { get; set; }

		public string SenderName { get; set; }

		public string RecipientName { get; set; }

		public string RecipientPhone { get; set; }

		public string RecipientAddress { get; set; }
	}
}