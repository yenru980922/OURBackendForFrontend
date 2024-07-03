using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.GoogleBooksApi
{
    public class GoogleBooksApiResponse
    {
        [JsonPropertyName("items")]
        public List<GoogleBookItem> Items { get; set; }
    }
}
