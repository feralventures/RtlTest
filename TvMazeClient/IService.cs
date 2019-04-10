using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeClient.Models;

namespace TvMazeClient
{
    public interface IService
    {
        Task<IDictionary<long, long>> GetUpdates(CancellationToken cancellationToken);
        Task<Show> GetShow(long showId, CancellationToken cancellationToken);
    }
}
