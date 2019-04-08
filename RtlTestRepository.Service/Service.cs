using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RtlTestRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace RtlTestRepository
{
    public class Service : IService
    {
        private readonly Context _context;

        public Service(Context context)
        {
            _context = context;
        }

        public async Task CreateOrUpdateShowsIncludingCast(IEnumerable<Show> observedShows, CancellationToken cancellationToken)
        {
            var queryExistingShows = _context.Show.Where(e => observedShows.Select(s => s.TvMazeId).Contains(e.TvMazeId)).Include(s => s.Cast);

            await queryExistingShows.LoadAsync(cancellationToken);

            _context.Person.RemoveRange(_context.Person.Local);
            _context.Show.RemoveRange(_context.Show.Local);
            await _context.SaveChangesAsync();

            _context.Show.AddRange(observedShows);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsIncludingCast(int skip, int take, CancellationToken cancellationToken)
        {
            var queryShows = _context.Show.Include(s => s.Cast).OrderBy(s => s.TvMazeId).Skip(skip).Take(take);

            var shows = await queryShows.ToListAsync(cancellationToken);

            return shows;
        }
    }
}
