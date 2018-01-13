using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using TempSoft.Newton.Interfaces.Calculator;

namespace TempSoft.Newton.Actors.Calculator
{
    [StatePersistence(StatePersistence.None)]
    public class CalculatorActor : Actor, ICalculatorActor
    {
        public CalculatorActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        public Task<decimal> Add(decimal a, decimal b)
        {
            return Task.FromResult(a + b);
        }
    }
}