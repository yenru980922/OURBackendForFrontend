using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.Dtos
{
    public class RefundsDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public DateTime ApplicationDate { get; set; }

        public int Amount { get; set; }
        public string RefundStatus { get; set; }


    }
}