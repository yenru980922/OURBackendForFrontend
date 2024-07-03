using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.Dtos
{
    public class AccessInfoDto
    {
        [JsonPropertyName("webReaderLink")]
        public string WebReaderLink { get; set; }
    }
}
