using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace TempSoft.Newton.Interfaces.Calculator
{
    public interface ICalculatorActor : IActor
    {
        Task<decimal> Add(decimal a, decimal b);
    }
}