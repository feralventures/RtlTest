using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMazeClient.Models;

namespace TvMazeClient
{
    public interface IService
    {
        Task<IEnumerable<(long ShowId, long Updated)>> GetUpdates(CancellationToken cancellationToken);
        Task<Show> GetShow(long showId, CancellationToken cancellationToken);
    }
}
