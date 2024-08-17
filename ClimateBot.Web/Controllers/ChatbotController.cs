using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClimateBot.Web.Services;
using System.Threading.Tasks;
using ClimateBot.Web.Models;
using System.Collections.Generic;
using System.Text;
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
            string city = ExtractCity(request.Question); // Extract or use last known city from session
            var climateData = await _climateService.GetClimateDataAsync(city);
            string response = BuildResponse(climateData, request.Question);
            return Ok(new { response });
        }

        private string ExtractCity(string question)
        {
            // Updated list of cities for example purposes, add more as needed
            var cities = new List<string> { "San José", "New York", "London", "Paris", "Tokyo" };

            foreach (var city in cities)
            {
                if (question.Contains(city, StringComparison.OrdinalIgnoreCase))
                {
                    HttpContext.Session.SetString("lastCity", city); // Update session if a new city is found
                    return city;
                }
            }

            return HttpContext.Session.GetString("lastCity") ?? "San José"; // Use last city from session or default
        }

        private string BuildResponse(ClimateData climateData, string question)
        {
            if (climateData == null)
            {
                return "Lo siento, no pude encontrar información sobre el clima para esa ubicación.";
            }

            var response = new StringBuilder($"En {climateData.Name}, la temperatura es de {climateData.Main.Temp}°C");
            if (climateData.Main.FeelsLike != 0) // Only add feels like if it's relevant
            {
                response.Append($" con una sensación térmica de {climateData.Main.FeelsLike}°C");
            }
            response.Append($". El clima es {climateData.Weather[0].Description} y el viento sopla a {climateData.Wind.Speed} m/s.");

            response.Append(" ¿Hay algo más que te gustaría saber?");
            return response.ToString();
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
