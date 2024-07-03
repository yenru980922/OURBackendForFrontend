using BackendForFrontend202401.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackendForFrontend.Models.Dtos
{

    public class OrderCreationDto
    {
        public OrdersDto OrdersDto { get; set; }
        public List<CartDetailsDto> OrderDetailsDto { get; set; }
    }
    public class OrdersDto
    {

        public int? Id { get; set; }
        public int MemberId { get; set; }
        public string? MemberName { get; set; }

        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; }

        public int? DiscountAmount { get; set; }


        public int TotalAmount { get; set; }

        public string Status { get; set; }
        public string Message { get; set; }

        public string Address { get; set; }

        public string? Phone { get; set; }
    }
    public class OrderDetailsDto
    {
        public int? OrderId { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        public int? Price { get; set; }


    }
   

}