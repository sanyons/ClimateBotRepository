using ClimateBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClimateBot.Services.StocksService;
namespace ClimateBot.Services
{
    public interface IStocksService
    {
        //ISP
        //Define una interfaz especifica para el servicio de Stocks
        Task<List<StockData>> GetStockDataAsync();
    }
}
