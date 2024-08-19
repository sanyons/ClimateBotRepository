using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TFPAW.ClimateBot.Web.Models;

namespace TFPAW.ClimateBot.Web.Services
{
    public class ClimateService : IClimateService
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiKey;
        private readonly string BaseUrl;

        public ClimateService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            ApiKey = configuration.GetValue<string>("OpenWeatherAPI:ApiKey");
            BaseUrl = configuration.GetValue<string>("OpenWeatherAPI:BaseUrl");
        }

        public async Task<ClimateData> GetClimateDataAsync(string city)
        {
            var requestUrl = $"{BaseUrl}?q={city}&appid={ApiKey}&units=metric";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var climateData = JsonSerializer.Deserialize<ClimateData>(jsonResponse, options);

            return climateData;
        }
    }
}
