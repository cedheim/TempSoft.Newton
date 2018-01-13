using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace TempSoft.Newton.Interfaces.Counter
{
    public interface ICounterService : IService
    {
        Task<long> CurrentCount();
    }
}