using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.ViewModels
{
    public class LogisticsOrdersVm
    {

        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }


        [Display(Name = "追蹤號碼")]
        public string TrackingNumber { get; set; }

        [Display(Name = "預計送達日期")]
        public DateTime EstimatedDeliveryDate { get; set; }

        [Display(Name = "實際送達日期")]
        public DateTime ActualDeliveryDate { get; set; }

        public string RecipientAddress { get; set; }

        public string RecipientPhone { get; set; }

     
        public string RecipientName { get; set; }


    }
}