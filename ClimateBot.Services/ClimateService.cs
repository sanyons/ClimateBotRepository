using ClimateBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClimateBot.Services
{
    //DIP
    //OCP
    // La clase ClimateService está abierta para extensión (nuevas funcionalidades) pero cerrada para modificación
    public class ClimateService : IClimateService
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiKey;
        private readonly string BaseUrl;

        public ClimateService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            ApiKey = configuration.GetValue<string>("ClimateAPI:ApiKey");
            BaseUrl = configuration.GetValue<string>("ClimateAPI:BaseUrl");
        }

        public async Task<ClimateData> GetClimateDataAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");
            request.Headers.Add("x-rapidapi-key", ApiKey);
            request.Headers.Add("x-rapidapi-host", "rapidweather.p.rapidapi.com");
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponse); // Para depuración

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var climateData = JsonSerializer.Deserialize<ClimateData>(jsonResponse, options);

            return climateData;

        }
    }
}
