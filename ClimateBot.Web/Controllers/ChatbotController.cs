using Microsoft.AspNetCore.Mvc;
using ClimateBot.Web.Services;
using System.Threading.Tasks;
using ClimateBot.Models;
using ClimateBot.Services;
using ClimateBot.Web.Models;


namespace ClimateBot.Web.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IClimateService _climateService;

        public ChatbotController(IClimateService climateService)
        {
            _climateService = climateService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            string city = ExtractCity(request.Question);
            var climateData = await _climateService.GetClimateDataAsync(city);

            string response = BuildResponse(climateData);

            return Ok(new { response });
        }

        private string ExtractCity(string question)
        {
            return "San José"; // Default si no se encuentra una ciudad en la pregunta
        }

        private string BuildResponse(ClimateData climateData)
        {
            return $"En {climateData.Name}, la temperatura es de {climateData.Main.Temp}°C con una sensación térmica de {climateData.Main.FeelsLike}°C. " +
                   $"El clima actual es {climateData.Weather[0].Description} con una velocidad del viento de {climateData.Wind.Speed} m/s.";
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
