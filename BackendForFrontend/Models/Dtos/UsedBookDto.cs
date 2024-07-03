using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.Dtos
{
    public class UsedBookDto
    {
        internal readonly string? Title;

        public int? Id { get; set; }

        public int? MemberId { get; set; }

        [Display(Name = "書名")]
        [JsonPropertyName("title")]
        public string? BookName { get; set; }

        
        public int? CategoryId { get; set; }


        [Display(Name = "作者")]
        [JsonPropertyName("authors")]
        public List<string>? Authors { get; set; }

        //額外屬性
        [Display(Name = "出版商")]
        [JsonPropertyName("publisher")]
        public string? PublisherName { get; set; }

        [JsonPropertyName("publishedDate")]

        [Display(Name = "出版日期")]
        public DateTime? PublishDate { get; set; }

        public bool? ProductStatus { get; set; }

        public decimal Price { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }


        [JsonPropertyName("identifier")]
        public string ISBN { get; set; }


        // 新增 ImageLinksDto 屬性
        [JsonPropertyName("imageLinks")]
        public ImageLinksDto ImageLinks { get; set; }

        // 新增試閱鏈接屬性
        [Display(Name = "試閱鏈接")]
        [JsonPropertyName("webReaderLink")]
        public string? WebReaderLink { get; set; }

        public string? Picture { get; set; }
        public IFormFile ImageFile { get; set; }


        public string BookStatus { get; set; }

        //會員資訊
        public string? MemberEmail { get; set; }

        //分類資訊
        [JsonPropertyName("kind")]
        public string? CategoryName { get; set; }

        [Display(Name = "上架日期")]
        public DateTime? ReleaseDate { get; set; } = DateTime.Now; // 這裡設置 ReleaseDate 屬性為當前時間

        public bool? IsDeleted { get; set; } // 新增的軟刪除標記
    }
}