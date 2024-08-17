using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClimateBot.Services
{
    // DESIGN_PATTERN: Factory Pattern
    // La clase NewsServiceFactory implementa el patron de diseño Factory para crear instancias de servicios especificos.
    // Este patron encapsula la logica de creacion de objetos y proporciona una interfaz comun para crear instancias de diferentes servicios.
    // LSP
    // La clase NewsServiceFactory implementa la interfaz INewsServiceFactory, lo que permite que el ServiceFactroy sea sustituido por cualquier otra implementacion de INewsServiceFactory sin alterar el comportamiento.
    public class NewsServiceFactory : INewsServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public NewsServiceFactory(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        // SRP
        // La responsabilidad de este método es crear instancias de NewsService.
        public INewsService CreateNewsService()
        {
            var httpClient = _httpClientFactory.CreateClient();
            return new NewsService(httpClient, _configuration);
        }
        // SRP
        // La responsabilidad de este método es crear instancias de ClimateService.
        public IClimateService CreateClimateService()
        {
            var httpClient = _httpClientFactory.CreateClient();
            return new ClimateService(httpClient, _configuration);
        }
        // SRP
        // La responsabilidad de este método es crear instancias de StockService.
        public IStocksService CreateStocksService()
        {
            var httpClient = _httpClientFactory.CreateClient();
            return new StocksService(httpClient, _configuration);
        }
    }
}
