using BackendForFrontend.Models.Dtos;
using System.Text.Json.Serialization;

namespace BackendForFrontend.Models.GoogleBooksApi
{
    public class GoogleBookItem
    {
        [JsonPropertyName("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }

        // 添加 AccessInfoDto 屬性來儲存試閱鏈接
        [JsonPropertyName("accessInfo")]
        public AccessInfoDto AccessInfo { get; set; }

    }
}
