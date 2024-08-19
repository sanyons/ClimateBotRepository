using ClimateBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace ClimateBot.Services {
    //DIP
    //OCP: La clase StocksService está abierta para extensión (nuevas funcionalidades) pero cerrada para modificación
    public class StocksService : IStocksService
{
    private readonly HttpClient _httpClient;
    private readonly string ApiKey;
    private readonly string BaseUrl;

    public StocksService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        ApiKey = configuration.GetValue<string>("StocksAPI:ApiKey");
        BaseUrl = configuration.GetValue<string>("StocksAPI:BaseUrl");
    }

    public async Task<List<StockData>> GetStockDataAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");
        request.Headers.Add("x-rapidapi-key", ApiKey);
        request.Headers.Add("x-rapidapi-host", "real-time-finance-data.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);

        Console.WriteLine("Done");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var stockDataResponse = JsonSerializer.Deserialize<StockDataResponse>(jsonResponse, options);

        if (stockDataResponse == null || stockDataResponse.Data?.Trends == null)
        {
            return new List<StockData>();
        }

        return stockDataResponse.Data.Trends;
    }

    //SRP
    // Unica responsabilidad de representar la estructura del api de Stocks
    public class StockDataResponse
    {
        public string Status { get; set; }
        public string RequestId { get; set; }
        public StockDataResponseData Data { get; set; }
    }

}
}
