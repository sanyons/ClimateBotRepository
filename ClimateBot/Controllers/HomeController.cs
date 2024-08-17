using Microsoft.AspNetCore.Mvc;

namespace ClimateBot.Controllers
{
    public class HomeController : Controller
    {
        // SOLID: Dependency Injection (DIP)
        // DESIGN_PATTERN: Dependency Injection Pattern
        private readonly INewsService _newsService;
        private readonly IClimateService _climateService;
        private readonly IStocksService _stocksService;

        //Constructor - DIP
        //Utiliza el patron de diseño de DI para recibir las interfaces necesarias
        //Permite al controlador depender de abstracciones en lugar de implementaciones concretas.
        public HomeController(INewsServiceFactory serviceFactory)
        {
            //OCP
            //El controlador esta abierto a la extension y cerrado a modificaciones
            _newsService = serviceFactory.CreateNewsService();
            _climateService = serviceFactory.CreateClimateService();
            _stocksService = serviceFactory.CreateStocksService();
        }

        //SRP
        //Se encarga solo de preparar y pasar el modelo de vista a las vistas.
        public async Task<IActionResult> Index()
        {
            //Obtiene los datos de los servicios
            var newsArticles = await _newsService.GetLatestNewsAsync();
            var climateData = await _climateService.GetClimateDataAsync();
            var stockData = await _stocksService.GetStockDataAsync();

            //Prepara el modelo
            var viewModel = new HomeViewModel
            {
                NewsArticles = newsArticles,
                ClimateData = climateData,
                StockData = stockData
            };

            //devuelve los datos a la vista
            return View(viewModel);
        }

        //SRP
        // Encapsula los datos necesarios para la vista.
        public class HomeViewModel
        {
            public List<NewsArticle> NewsArticles { get; set; }
            public ClimateData ClimateData { get; set; }
            public List<StockData> StockData { get; set; }
        }
    }
}
