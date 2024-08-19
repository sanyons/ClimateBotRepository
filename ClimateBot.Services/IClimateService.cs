using ClimateBot.Web.Services;
using ClimateBot.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;


using System.Threading.Tasks;


namespace ClimateBot.Web.Services
{
    public interface IClimateService
    {
        Task<ClimateData> GetClimateDataAsync(string city);
    
    }
}
