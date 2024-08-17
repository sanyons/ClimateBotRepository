using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateBot.Services
{
    // DESIGN_PATTERN: Factory Pattern
    //Aprovechamos el factory para poder usar un mismo servicio para poder pasar el resto de la informacion necesaria de los otros apis mapeados.
    public interface INewsServiceFactory
    {
        INewsService CreateNewsService();
        IClimateService CreateClimateService();
        IStocksService CreateStocksService();

    }
}
