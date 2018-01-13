using System;
using System.Diagnostics.Tracing;
using System.Threading;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Runtime;
using TempSoft.Newton.Services.Calculator;
using TempSoft.Newton.Services.Counter;

namespace TempSoft.Newton.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Creating a new event listener to redirect the traces to a file
                var listener = new ServiceEventListener("ServicesApplication");
                listener.EnableEvents(ServiceEventSource.Current, EventLevel.LogAlways, EventKeywords.All);

                // This line registers an Actor Service to host your actor class with the Service Fabric runtime.

                ServiceRuntime.RegisterServiceAsync("CalculatorServiceType", context => new CalculatorService(context)).GetAwaiter().GetResult();
                ServiceRuntime.RegisterServiceAsync("CounterServiceType", context => new CounterService(context, new ReliableStateManager(context))).GetAwaiter().GetResult();

                //ActorRuntime.RegisterActorAsync<CalculatorActor>((context, actorType) => new ActorService(context, actorType)).GetAwaiter().GetResult();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(ex.ToString());
                throw;
            }
        }
    }
}
