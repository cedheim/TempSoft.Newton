using System;
using System.Diagnostics.Tracing;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Runtime;
using TempSoft.Newton.Actors.Calculator;

namespace TempSoft.Newton.Actors
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Creating a new event listener to redirect the traces to a file
                var listener = new ActorEventListener("ActorApplication");
                listener.EnableEvents(ActorEventSource.Current, EventLevel.LogAlways, EventKeywords.All);

                // This line registers an Actor Service to host your actor class with the Service Fabric runtime.
                ActorRuntime.RegisterActorAsync<CalculatorActor>((context, actorType) => new ActorService(context, actorType)).GetAwaiter().GetResult();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(ex.ToString());
                throw;
            }
        }
    }
}
