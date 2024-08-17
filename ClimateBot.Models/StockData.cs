using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateBot.Models
{
    public class StockData
    {
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Change { get; set; }
        public float ChangePercent { get; set; }
        public float PreviousClose { get; set; }
        public float pre_or_post_market_change { get; set; }
        public float pre_or_post_market_change_percent { get; set; }
        public string LastUpdateUtc { get; set; }
        public string ExchangeOpen { get; set; }
        public string ExchangeClose { get; set; }
        public string Timezone { get; set; }
        public float UtcOffsetSec { get; set; }
        public string GoogleMid { get; set; }
    }

    public class StockDataResponseData
    {
        public List<StockData> Trends { get; set; }
        public List<StockArticle> News { get; set; }

    }

    public class StockArticle
    {
        public string ArticleTitle { get; set; }
        public string ArticleUrl { get; set; }
        public string ArticlePhotoUrl { get; set; }
        public string Source { get; set; }
        public string PostTimeUtc { get; set; }
        public List<StockDataInNews> StocksInNews { get; set; }
    }

    public class StockDataInNews
    {
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Change { get; set; }
        public float ChangePercent { get; set; }
        public float PreviousClose { get; set; }
        public float pre_or_post_market_change { get; set; }
        public float pre_or_post_market_change_percent { get; set; }
        public string LastUpdateUtc { get; set; }
        public string ExchangeOpen { get; set; }
        public string ExchangeClose { get; set; }
        public string Timezone { get; set; }
        public float UtcOffsetSec { get; set; }
        public string GoogleMid { get; set; }
    }
}
