using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.Dtos
{
    public class ReturnsDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MemberId { get; set; }

        public int LogisticsOrderId { get; set; }

        public int Quantity { get; set; }

        public string ReturnReason { get; set; }

        public string Status { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime ProcessdDate { get; set; }


    }
}