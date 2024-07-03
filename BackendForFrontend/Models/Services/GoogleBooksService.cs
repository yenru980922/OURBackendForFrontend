using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.GoogleBooksApi; // 確保引入正確的命名空間
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using BackendForFrontend.Models.Services;

namespace BackendForFrontend.Services
{
    public class GoogleBooksService : IGoogleBooksService
    {
        private readonly HttpClient _httpClient;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsedBookDto> GetBookByISBNAsync(string isbn)
        {
            var response = await _httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                var googleApiResponse = JsonSerializer.Deserialize<GoogleBooksApiResponse>(content); // 使用正確的反序列化類型

                var firstItem = googleApiResponse.Items?.FirstOrDefault(); // 使用 ?. 安全訪問 Items
                if (firstItem == null) return null;

                var volumeInfo = firstItem.VolumeInfo;
                var accessInfo = firstItem.AccessInfo;
                if (volumeInfo != null)
                {
                    return new UsedBookDto
                    {
                        BookName = volumeInfo.Title,
                        Authors = volumeInfo.Authors,
                        PublisherName = volumeInfo.Publisher,
                        PublishDate = DateTime.TryParse(volumeInfo.PublishedDate, out var date) ? date : null,
                        Description = volumeInfo.Description,
                        ISBN = isbn, // 直接使用方法參數中的 ISBN
                        ImageLinks = volumeInfo.ImageLinks,
                        WebReaderLink = accessInfo.WebReaderLink ?? ""
                        // 需要額外處理 Picture 和可能的 CategoryName
                    };
                }
                return null;
            }
            else
            {
                return null; // 或處理錯誤情況
            }
        }
    }
}
