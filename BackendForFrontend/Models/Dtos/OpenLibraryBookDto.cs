using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.Dtos
{
    public class OpenLibraryBookDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("authors")]
        public List<OpenLibraryAuthorDto> Authors { get; set; } = new List<OpenLibraryAuthorDto>();

        [JsonPropertyName("publishers")]
        public List<string> Publishers { get; set; } = new List<string>();

        [JsonPropertyName("publish_date")]
        public string PublishDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        // Open Library可能返回封面图片的URL在一个不同的属性中
        [JsonPropertyName("cover")]
        public OpenLibraryCoverDto Cover { get; set; }

        // 根据需要添加更多字段
    }
}
