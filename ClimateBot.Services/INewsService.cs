using ClimateBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateBot.Services
{
    public interface INewsService
    {
        // SOLID: Interface Segregation Principle (ISP)
        // Define una interfaz específica para el servicio de noticias
        Task<List<NewsArticle>> GetLatestNewsAsync();
    }
}
