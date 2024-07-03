using BackendForFrontend.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{
    public class PdInStockDto
    {
        public int ID { get; set; }

        public int ProductId { get; set; }

        public int SupplierID { get; set; }

        public int Qty { get; set; }

        public DateTime BuyDate { get; set; }

        public decimal BuyPrice { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }
        public string BooksellerName { get; set; }
        public int Stock { get; set; }
    }
}