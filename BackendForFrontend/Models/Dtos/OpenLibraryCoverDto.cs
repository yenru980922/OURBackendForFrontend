using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.Dtos
{
    public class OpenLibraryCoverDto
    {
        [JsonPropertyName("small")]
        public string Small { get; set; }

        [JsonPropertyName("medium")]
        public string Medium { get; set; }

        [JsonPropertyName("large")]
        public string Large { get; set; }
    }
}
