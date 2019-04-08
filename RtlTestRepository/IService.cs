using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public interface IService
    {
        Task<IEnumerable<Show>> GetShowsIncludingCast(int skip, int take, CancellationToken cancellationToken);
        Task CreateOrUpdateShowsIncludingCast(IEnumerable<Show> shows, CancellationToken cancellationToken);
    }
}
