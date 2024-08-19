using Microsoft.AspNetCore.Mvc;
using TFPAW.ClimateBot.Services;
namespace TFPAW.ClimateBot.Web.Controllers
{
    public class NewsController : Controller
    {
        // SOLID: Dependency Injection (DIP)
        // DESIGN_PATTERN: Dependency Injection Pattern
        private readonly INewsService _newsService;

        // Constructor que recibe INewsService via Dependency Injection
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // Método de acción para la ruta raíz /News/Index
        public async Task<IActionResult> Index()
        {
            // Llama al servicio para obtener las últimas noticias de manera asíncrona
            var newsArticles = await _newsService.GetLatestNewsAsync();

            // Devuelve una vista con los artículos de noticias obtenidos
            return View(newsArticles);
        }
    }
}
