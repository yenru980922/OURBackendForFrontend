namespace BackendForFrontend.Models.Dtos
{
    public class UsedBookCartsDto
    {
        public int Id { get; set; }

        public int BookID { get; set; }

        public string Name { get; set; }

        public string? Picture { get; set; }

        public bool ProductStatus { get; set; } //true:上架中

        public decimal UnitPrice { get; set; }

        public string BookStatus { get; set; }

        public string SellerName {  get; set; }

        public int SellerID { get; set; }
    }
}
