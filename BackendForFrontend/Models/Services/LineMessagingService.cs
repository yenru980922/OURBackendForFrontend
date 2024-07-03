using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BackendForFrontend.Models.Services;
using Microsoft.Extensions.Configuration;

public class LineMessagingService : ILineMessagingService
{
    private readonly HttpClient _httpClient;
    private readonly string _lineAccessToken;

    public LineMessagingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // 從appsettings.json或你的配置源中讀取Access Token
        _lineAccessToken = configuration["LineSettings:AccessToken"];
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _lineAccessToken);
    }

    public async Task SendPushMessageAsync(string userId, string message)
    {
        var url = "https://api.line.me/v2/bot/message/push";
        var data = $@"{{
            ""to"": ""{userId}"",
            ""messages"": [
                {{
                    ""type"": ""text"",
                    ""text"": ""{message}""
                }}
            ]
        }}";

        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            // 處理錯誤回應
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to send LINE message: {errorContent}");
        }
    }
}
