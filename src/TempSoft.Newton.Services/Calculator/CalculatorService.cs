using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using TempSoft.Newton.Interfaces.Calculator;

namespace TempSoft.Newton.Services.Calculator
{
    public class CalculatorService : StatelessService, ICalculatorService
    {
        public CalculatorService(StatelessServiceContext serviceContext) : base(serviceContext)
        {
        }

        public async Task<decimal> Add(decimal a, decimal b)
        {
            return await Task.Run(() => a + b);
        }


        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}