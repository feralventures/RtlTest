using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeClient.Models;

namespace TvMazeClient
{
    public interface IService
    {
        Task<IEnumerable<Show>> GetShows(long pageIndex, CancellationToken cancellationToken);
        Task<IEnumerable<Actor>> GetCast(long showId, CancellationToken cancellationToken);
    }
}
