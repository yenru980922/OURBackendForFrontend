using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.Dtos
{
    public class ImageLinksDto
    {
        [JsonPropertyName("smallThumbnail")]
        public string SmallThumbnailPicture { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailPicture { get; set; }

        [JsonPropertyName("small")]
        public string SmallPicture { get; set; }


    }
}
