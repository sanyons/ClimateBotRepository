using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClimateBot.Web.Services;
using System.Threading.Tasks;
using ClimateBot.Web.Models;
using System.Text;
using System.Text.RegularExpressions;
using System;

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
            string response = BuildResponse(climateData, request.Question);
            return Ok(new { response });
        }

        private string ExtractCity(string question)
        {
            var cityRegex = new Regex(@"\b(San José|New York|London|Paris|Tokyo)\b", RegexOptions.IgnoreCase);
            var match = cityRegex.Match(question);
            if (match.Success)
            {
                var city = match.Value;
                HttpContext.Session.SetString("lastCity", city); // Update the last city in the session
                return city;
            }

            // Fallback to the last known city if no new city is found in the question
            return HttpContext.Session.GetString("lastCity") ?? "San José"; // Default if no city found ever
        }

        private string BuildResponse(ClimateData climateData, string question)
        {
            string intent = DetermineIntent(question);
            switch (intent)
            {
                case "greeting":
                    return "Hola, ¿cómo puedo ayudarte con el clima hoy?";
                case "goodbye":
                    return "Adiós, ¡espero haber sido de ayuda!";
                case "info":
                    return "Puedo darte información actualizada sobre el clima en varias ciudades. Solo pregúntame.";
                case "weather":
                default:
                    if (climateData == null)
                    {
                        return "Lo siento, no pude encontrar información sobre el clima para esa ubicación.";
                    }
                    var response = new StringBuilder($"En {climateData.Name}, la temperatura es de {climateData.Main.Temp}°C.");
                    response.Append($" El clima es {climateData.Weather[0].Description} y el viento sopla a {climateData.Wind.Speed} m/s.");
                    response.Append(" ¿Hay algo más que te gustaría saber?");
                    return response.ToString();
            }
        }

        private string DetermineIntent(string question)
        {
            if (question.Contains("hola", StringComparison.OrdinalIgnoreCase) ||
                question.Contains("hi", StringComparison.OrdinalIgnoreCase))
            {
                return "greeting";
            }
            if (question.Contains("adios", StringComparison.OrdinalIgnoreCase) ||
                question.Contains("bye", StringComparison.OrdinalIgnoreCase))
            {
                return "goodbye";
            }
            if (question.Contains("qué más sabes", StringComparison.OrdinalIgnoreCase) ||
                question.Contains("info", StringComparison.OrdinalIgnoreCase))
            {
                return "info";
            }
            return "weather";
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
