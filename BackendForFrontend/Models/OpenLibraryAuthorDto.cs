using System.Text.Json.Serialization;

namespace BackendForFrontend.Models
{
    public class OpenLibraryAuthorDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
