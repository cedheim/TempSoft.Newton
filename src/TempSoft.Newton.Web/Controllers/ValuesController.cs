using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using TempSoft.Newton.Interfaces.Calculator;
using TempSoft.Newton.Interfaces.Counter;

namespace TempSoft.Newton.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random());

        public ValuesController()
        {
        }

        // GET api/values
        [HttpGet]
        public async Task<long> Get()
        {
            try
            {
                var bytes = new byte[8];

                _random.Value.NextBytes(bytes);
                var key = BitConverter.ToInt64(bytes, 0);

                var proxy = ServiceProxy.Create<ICounterService>(new Uri("fabric:/TempSoft.Newton/CounterService"), partitionKey: new ServicePartitionKey(key));

                return await proxy.CurrentCount();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        // GET api/values/5
        [HttpGet("{a}/{b}")]
        public async Task<decimal> CalculatorService(decimal a, decimal b)
        {
            var proxy = ServiceProxy.Create<ICalculatorService>(new Uri("fabric:/TempSoft.Newton/CalculatorService"), partitionKey: ServicePartitionKey.Singleton);

            return await proxy.Add(a, b);
        }

        // GET api/values/5
        [HttpGet("{id}/{a}/{b}")]
        public async Task<decimal> ActorService(Guid id, decimal a, decimal b)
        {
            var proxy = ActorProxy.Create<ICalculatorActor>(new ActorId(id));
            
            return await proxy.Add(a, b);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
