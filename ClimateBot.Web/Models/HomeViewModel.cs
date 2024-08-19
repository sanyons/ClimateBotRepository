using ClimateBot.Models;
using static ClimateBot.Services.StocksService;

namespace ClimateBot.Web.Models
{
    public class HomeViewModel
    {
        public List<NewsArticle> NewsArticles { get; set; }
        public ClimateData ClimateData { get; set; }
        public List<StockData> StockData { get; set; }
    }
}
