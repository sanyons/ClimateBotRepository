using TFPAW.ClimateBot.Models;
using static TFPAW.ClimateBot.Services.StocksService;

namespace TFPAW.ClimateBot.Web.Models
{
    public class HomeViewModel
    {
        public List<NewsArticle> NewsArticles { get; set; }
        public ClimateData ClimateData { get; set; }
        public List<StockData> StockData { get; set; }
    }
}
