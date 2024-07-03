namespace BackendForFrontend202401.Models.Dtos
{
    public class CartsDto
    {

        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public string PaymentMethod { get; set; }

        public decimal? DiscountAmount { get; set; }


        public decimal TotalAmount { get; set; }

        public string Message { get; set; }

        public int? Phone { get; set; }

        public string Address { get; set; }
    }
}