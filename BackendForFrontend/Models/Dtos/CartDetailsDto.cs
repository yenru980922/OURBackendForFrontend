using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.Dtos
{
    public class CartDetailsDto
    {
        public int? Id { get; set; }
        public int CartId { get; set; }

        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
       
       
    }
}