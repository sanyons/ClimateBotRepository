using TFPAW.ClimateBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace TFPAW.ClimateBot.Services
{
    // SOLID: Dependency Inversion Principle (DIP)
    // SOLID: Open/Closed Principle (OCP)
    // La clase NewsService está abierta para extensión (nuevas funcionalidades) pero cerrada para modificación
    // DESIGN_PATTERN: Strategy Pattern
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiKey;
        private readonly string BaseUrl;

        // Constructor that uses IConfiguration
        public NewsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            ApiKey = configuration.GetValue<string>("NewsAPI:ApiKey");
            BaseUrl = configuration.GetValue<string>("NewsAPI:BaseUrl");
        }

        public async Task<List<NewsArticle>> GetLatestNewsAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}{ApiKey}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var newsResponse = JsonSerializer.Deserialize<NewsApiResponse>(jsonResponse, options);

            if (newsResponse?.Results == null)
            {
                return new List<NewsArticle>();
            }

            return newsResponse.Results.Select(article => new NewsArticle
            {
                Title = article.Title,
                Description = article.Description,
                Url = article.Url,
                PublishedDate = article.PublishedDate,
                Source = article.Source
            }).ToList();
        }

        //SRP
        // Unica responsabilidad de representar la estructura del api de noticias
        internal class NewsApiResponse
        {
            public List<NewsArticle> Results { get; set; }
        }
    }
}
