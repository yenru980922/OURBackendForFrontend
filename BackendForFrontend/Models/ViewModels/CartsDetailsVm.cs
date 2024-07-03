using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.ViewModels
{
    public class CartsDetailsVm
    {
        [Display(Name = "商品")]
        public string ProductName { get; set; }


        [Display(Name = "數量")]
        public int Quantity { get; set; }


        [Display(Name = "單價")]
        public int UnitPrice { get; set; }
    }
}