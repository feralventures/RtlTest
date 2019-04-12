using System.Threading;
using System.Threading.Tasks;

namespace RtlTestLogic
{
    public interface IService
    {
        Task Execute(long workloadSize, CancellationToken cancellationToken);
    }
}