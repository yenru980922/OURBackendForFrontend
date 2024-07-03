namespace BackendForFrontend.Models.Dtos.Product
{
    public class ProductFilter
    {
        public string? Keyword { get; set; }

        public int BookSearch { get; set; }

        public string PhysicalBook { get; set; }

        public string EBook { get; set; }

        public int PriceRangeStart { get; set; } = 0;

        public int PriceRangeEnd { get; set; } = 10000;

        public int? ProductDetailsCategoryId { get; set; } = 0;

        public int? Page { get; set; } = 1; //第一頁
        public int? pageSize { get; set; } = 9; //一頁顯示9筆資料
    }
}
