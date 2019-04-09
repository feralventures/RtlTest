using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RtlTestRepository.Models;

namespace RtlTestRepository
{
    public class Service : IService
    {
        private readonly Context _context;

        public Service(Context context)
        {
            _context = context;
        }

        public async Task CreateShow(Show show, CancellationToken cancellationToken)
        {
            _context.Show.AddRange(show);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Show>> GetShows(int skip, int take, CancellationToken cancellationToken)
        {
            var queryShows = _context.Show.Include(s => s.Cast).OrderBy(s => s.TvMazeId).Skip(skip).Take(take);

            var shows = await queryShows.ToListAsync(cancellationToken);

            return shows;
        }

        public async Task<IEnumerable<(long TvMazeId, long Updated)>> GetUpdates(CancellationToken cancellationToken)
        {
            var queryUpdates = _context.Show.Select(s => new
            {
                s.TvMazeId,
                s.Updated
            });

            var updates = (await queryUpdates
                .ToListAsync(cancellationToken))
                .Select(u => (u.TvMazeId, u.Updated));

            return updates;
        }

        public async Task UpdateShow(Show show, CancellationToken cancellationToken)
        {
            var queryExistingShows = _context.Show.Where(es => es.TvMazeId == show.TvMazeId).Include(s => s.Cast);
            await queryExistingShows.LoadAsync(cancellationToken);

            _context.Show.RemoveRange(_context.Show.Local);
            await _context.SaveChangesAsync(cancellationToken);

            _context.Show.AddRange(show);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
