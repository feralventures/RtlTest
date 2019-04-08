using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public class Mock : IService
    {
        public async Task CreateOrUpdateShowsIncludingCast(IEnumerable<Show> shows, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
        }

        public async Task<IEnumerable<Show>> GetShowsIncludingCast(int skip, int take, CancellationToken cancellationToken)
        {
            await Task.Delay(0);

            return new List<Show>();
        }
    }
}
