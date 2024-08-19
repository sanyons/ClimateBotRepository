using TFPAW.ClimateBot.Web.Services;
using TFPAW.ClimateBot.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;


using System.Threading.Tasks;


namespace TFPAW.ClimateBot.Web.Services
{
    public interface IClimateService
    {
        Task<ClimateData> GetClimateDataAsync(string city);
    
    }
}
