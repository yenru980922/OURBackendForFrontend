namespace BackendForFrontend.Models.Dtos.Product
{
    public class ProductsPagingDto
    {
        public int TotalPages { get; set; }
        public List<BookProductDto>? ProductsReslut { get; set; }
        public List<ProductDetailsCategoryDto> ProductDetailsCategories { get; set; }
    }
}
