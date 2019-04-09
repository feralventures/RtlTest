using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public interface IService
    {
        Task<IEnumerable<(long TvMazeId, long Updated)>> GetUpdates(CancellationToken cancellationToken);
        Task<IEnumerable<Show>> GetShows(int skip, int take, CancellationToken cancellationToken);
        Task CreateShow(Show show, CancellationToken cancellationToken);
        Task UpdateShow(Show show, CancellationToken cancellationToken);
    }
}
