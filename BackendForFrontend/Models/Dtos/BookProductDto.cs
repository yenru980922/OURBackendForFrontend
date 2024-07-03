using BackendForFrontend.Controllers;
using System.ComponentModel.DataAnnotations;

namespace BackendForFrontend.Models.Dtos
{
    public class BookProductDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public int ProductId { get; set; }
        public int PublisherId { get; set; }

        [Display(Name = "出版日期")]
        public DateTime? PublishDate { get; set; }
        public string ISBN { get; set; }

        //額外屬性
        [Display(Name = "出版商")]
        public string PublisherName { get; set; }

        //Book
        [Display(Name = "書名")]
        public string Author { get; set; }
        public string BookLanguage { set; get; }

        //Product
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<string> ImageUrl { get; set; }
        public decimal Price { get; set; }

        public decimal? RealPrice { get; set; }

        public int? DiscountDegree { get; set; }

        public string ProductStatus { get; set; }

        public string Description { get; set; }
        public int Stock { get; set; }

        public int? DetailsCategoryId { get; set; }

        public string DetailsCategoryName { get; set; }

        public IEnumerable<ProductKeywordDto>? ProductKeywords { get; set; }
    }
}