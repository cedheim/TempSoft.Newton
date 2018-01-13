using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace TempSoft.Newton.Interfaces.Calculator
{
    public interface ICalculatorService : IService
    {
        Task<decimal> Add(decimal a, decimal b);
    }
}
