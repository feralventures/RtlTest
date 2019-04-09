using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public class Mock : IService
    {
        public Task CreateShow(Show show, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Show>> GetShows(int skip, int take, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(long TvMazeId, long Updated)>> GetUpdates(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShow(Show show, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
