namespace BackendForFrontend.Models.Dtos
{
    public class ProductsFilterDto
    {
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        public int? Stock { get; set; }
    }
}