using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public class Mock : IService
    {
        public Task PersistShow(Show show, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Show>> GetShows(int skip, int take, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<long , long>> GetUpdates(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
