using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.Dtos
{
    public class LogisticsOrdersDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public string TrackingNumber { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        public DateTime ActualDeliveryDate { get; set; }

        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }

        public string RecipientAddress { get; set; }

    }
}