using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CryptoBlazor.Data
{
    public class ThemeService : IThemeService
    {
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ThemeService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            httpClient.BaseAddress = new Uri(_appSettings.CryptoAPIBaseAddress);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");

            _httpClient = httpClient;
        }
        public async Task<Theme> GetTheme(short id)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "Themes/" + id);
            httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await _httpClient.SendAsync(httpRequestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            var returnedTheme = JsonConvert.DeserializeObject<Theme>(responseBody);

            return await Task.FromResult(returnedTheme);
        }
    }
}
