﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClimateBot.Web.Services;
using System.Threading.Tasks;
using ClimateBot.Web.Models;
using System.Text;

namespace ClimateBot.Web.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IClimateService _climateService;
        private readonly NLPService _nlpService;
        private readonly ILogger<ChatbotController> _logger;

        public ChatbotController(IClimateService climateService, NLPService nlpService, ILogger<ChatbotController> logger)
        {
            _climateService = climateService;
            _nlpService = nlpService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            _logger.LogInformation("Received question: {Question}", request.Question);

            try
            {
                var nlpResponse = await _nlpService.GetNLPResponseAsync(request.Question);
                string intent = nlpResponse.Intent;
                string city = nlpResponse.City;

                HttpContext.Session.SetString("lastIntent", intent);

                if (intent == "unknown")
                {
                    _logger.LogWarning("Could not determine intent for question: {Question}", request.Question);
                    return Ok(new { response = "No estoy seguro de cómo ayudarte con eso. ¿Puedes especificar si quieres saber sobre el clima o necesitas ayuda con otra cosa?" });
                }

                if (intent == "greeting")
                {
                    return Ok(new { response = "Hola, ¿cómo puedo ayudarte hoy? ¿Quieres saber el clima de alguna ciudad específica?" });
                }

                if (intent == "goodbye")
                {
                    return Ok(new { response = "¡Gracias por usar nuestro servicio de chat! Espero haber sido útil. ¡Hasta luego!" });
                }

                if (intent == "thanks")
                {
                    return Ok(new { response = "¡De nada! Estoy aquí para ayudarte. ¿Hay algo más que te gustaría saber?" });
                }

                if (string.IsNullOrEmpty(city) && intent == "weather")
                {
                    city = HttpContext.Session.GetString("lastCity");

                    if (string.IsNullOrEmpty(city))
                    {
                        _logger.LogWarning("Could not extract city from question: {Question}", request.Question);
                        return Ok(new { response = "No pude determinar la ciudad a partir de tu mensaje. ¿Podrías especificar más claramente?" });
                    }
                }

                var climateData = await _climateService.GetClimateDataAsync(city);
                if (climateData == null || string.IsNullOrWhiteSpace(climateData.Name))
                {
                    _logger.LogError("Failed to retrieve climate data for city: {City}", city);
                    return Ok(new { response = $"No pude encontrar información sobre el clima para {city}. ¿Podrías verificar el nombre de la ciudad?" });
                }

                HttpContext.Session.SetString("lastCity", city);
                string response = BuildResponse(climateData, request.Question, intent);
                _logger.LogInformation("Successfully processed request for city: {City}", city);
                return Ok(new { response });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, new { response = "Lo siento, hubo un problema al procesar tu solicitud. Por favor, intenta nuevamente." });
            }
        }

        private string BuildResponse(ClimateData climateData, string question, string intent)
        {
            var lastIntent = HttpContext.Session.GetString("lastIntent");

            if (intent == "greeting" && lastIntent != "weather")
            {
                return "Hola, ¿cómo puedo ayudarte hoy? ¿Quieres saber el clima de alguna ciudad específica?";
            }
            if (intent == "goodbye")
            {
                return "¡Gracias por usar nuestro servicio de chat! Espero haber sido útil. ¡Hasta luego!";
            }
            if (intent == "info")
            {
                return "Puedo proporcionarte información sobre el clima de cualquier ciudad. Solo dime el nombre de la ciudad y te diré cómo está el clima allí.";
            }

            var response = new StringBuilder($"En {climateData.Name}, {climateData.Sys.Country}, la temperatura es de {climateData.Main.Temp}°C. ");
            response.Append($"El clima es {climateData.Weather[0].Description} y el viento sopla a {climateData.Wind.Speed} m/s.");

            if (question.ToLower().Contains("viento"))
            {
                response.Append($" Además, la velocidad del viento es de {climateData.Wind.Speed} m/s.");
            }
            if (question.ToLower().Contains("humedad"))
            {
                response.Append($" La humedad relativa es del {climateData.Main.Humidity}%.");
            }

            response.Append(" ¿Hay algo más que te gustaría saber?");
            return response.ToString();
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
