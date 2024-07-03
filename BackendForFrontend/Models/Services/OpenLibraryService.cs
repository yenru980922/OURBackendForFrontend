using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.GoogleBooksApi;
using BackendForFrontend.Models.Services;
using BackendForFrontend.Services;
using System.Net.Http;
using System.Text.Json;

public class OpenLibraryService : IOpenLibraryService
{
    private readonly HttpClient _httpClient;

    public OpenLibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<UsedBookDto> GetBookByISBNAsync(string isbn)
    {
        var response = await _httpClient.GetAsync($"https://openlibrary.org/isbn/{isbn}.json");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var bookData = JsonSerializer.Deserialize<OpenLibraryBookDto>(content);

            if (bookData == null) return null;

            return new UsedBookDto
            {
                BookName = bookData.Title,
                Authors = bookData.Authors?.Select(a => a.Name).ToList() ?? new List<string>(),
                PublisherName = bookData.Publishers.FirstOrDefault(),
                PublishDate = DateTime.TryParse(bookData.PublishDate, out var date) ? date : null, // 注意: Open Library 的日期可能需要格式化
                Description = bookData.Description,
                ISBN = isbn,
                // 其他字段的轉換...
            };
        }
        return null; // 如果API響應不成功，返回null
    }
}
