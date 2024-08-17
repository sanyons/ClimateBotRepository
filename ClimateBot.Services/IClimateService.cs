using ClimateBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateBot.Services
{
    public interface IClimateService
    {
        // ISP
        // Define interfaz especifica para el servicio del clima
        Task<ClimateData> GetClimateDataAsync();
    }
}

