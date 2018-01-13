using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TempSoft.Newton.Interfaces.Counter;

namespace TempSoft.Newton.Services.Counter
{
    public class CounterService : StatefulService, ICounterService
    {
        public CounterService(StatefulServiceContext serviceContext) : base(serviceContext)
        {
        }

        public CounterService(StatefulServiceContext serviceContext, IReliableStateManagerReplica2 reliableStateManagerReplica) : base(serviceContext, reliableStateManagerReplica)
        {
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    var counterValue = await myDictionary.GetOrAddAsync(tx, "Counter", 0);
                    
                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        public async Task<long> CurrentCount()
        {
            ConditionalValue<long> result;
            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");
            using (var tx = this.StateManager.CreateTransaction())
            {
                result = await myDictionary.TryGetValueAsync(tx, "Counter");
                await tx.CommitAsync();
            }
            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return -1;
            }
        }
    }
}