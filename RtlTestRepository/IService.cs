using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public interface IService
    {
        Task<IDictionary<long , long>> GetUpdates(CancellationToken cancellationToken);
        Task<IEnumerable<Show>> GetShows(int skip, int take, CancellationToken cancellationToken);
        Task PersistShow(Show show, CancellationToken cancellationToken);
    }
}
