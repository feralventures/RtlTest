using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RtlTestLogic
{
    public class Service : IService
    {
        private readonly TvMazeClient.IService _tvMazeClient = null;
        private readonly Func<RtlTestRepository.IService> _rtlTestRepositoryFactory = null;

        public Service(TvMazeClient.IService tvMazeClient, Func<RtlTestRepository.IService> rtlTestRepositoryFactory)
        {
            _tvMazeClient = tvMazeClient;
            _rtlTestRepositoryFactory = rtlTestRepositoryFactory;
        }

        public async Task Execute(long workloadSize, CancellationToken cancellationToken)
        {
            var workload = await DetermineWorkload(workloadSize, cancellationToken);
            ProcessWorkload(workload, cancellationToken);
        }

        private async Task<IEnumerable<long>> DetermineWorkload(long workloadSize, CancellationToken cancellationToken)
        {
            List<long> workload = new List<long>();

            IDictionary<long, long> tvMazeUpdates = await _tvMazeClient.GetUpdates(cancellationToken);
            IDictionary<long, long> rtlTestUpdates = await _rtlTestRepositoryFactory().GetUpdates(cancellationToken);

            for (var updatesToScan = tvMazeUpdates.AsEnumerable(); (workload.Count() < workloadSize) && updatesToScan.Any(); updatesToScan = updatesToScan.Skip(1))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var tvMazeUpdate = updatesToScan.First();

                if (rtlTestUpdates.TryGetValue(tvMazeUpdate.Key, out var updated))
                {
                    if (updated < tvMazeUpdate.Value)
                    {
                        workload.Add(tvMazeUpdate.Key);
                    }
                }
                else
                {
                    workload.Add(tvMazeUpdate.Key);
                }
            }

            return workload;
        }

        private void ProcessWorkload(IEnumerable<long> workload, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Parallel.ForEach(workload, new ParallelOptions() { CancellationToken = cancellationToken }, async (work) =>
                {
                    var s = await _tvMazeClient.GetShow(work, cancellationToken);

                    var observedShow = new RtlTestRepository.Models.Show
                    {
                        TvMazeId = s.Id,
                        Updated = s.Updated,
                        Name = s.Name,
                        Cast = s.Embedded.Cast.GroupBy(a => a.Person.Id)
                        .Select(ga => ga.First())
                        .Select(a => new RtlTestRepository.Models.Person
                        {
                            TvMazeId = a.Person.Id,
                            Name = a.Person.Name,
                            Birthday = a.Person.Birthday
                        }).ToList(),
                    };

                    await _rtlTestRepositoryFactory().PersistShow(observedShow, cancellationToken);
                });
            };
        }
    }
}
