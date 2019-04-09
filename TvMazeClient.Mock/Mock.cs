using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TvMazeClient.Models;

namespace TvMazeClient
{
    public class Mock : IService
    {
        readonly Assembly assembly = Assembly.GetExecutingAssembly();

        private T GetResource<T>(string resourceName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        private readonly Dictionary<long, Show> shows;
        private readonly Dictionary<long, Actor[]> cast;

        public Mock(string baseUri)
        {
            shows = new Dictionary<long, Show>()
            {
                { 1, GetResource<Show>("TvMazeClient.Mock.Resources.Show_1.json") },
                { 2, GetResource<Show>("TvMazeClient.Mock.Resources.Show_2.json") },
                { 3, GetResource<Show>("TvMazeClient.Mock.Resources.Show_3.json") },
                { 4, GetResource<Show>("TvMazeClient.Mock.Resources.Show_4.json") }
            };
        }

        public async Task<Show> GetShow(long showId, CancellationToken cancellationToken)
        {
            Show show;

            if (shows.TryGetValue(showId, out show))
            {
                return await Task.FromResult<Show>(show);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<(long ShowId, long Updated)>> GetUpdates(CancellationToken cancellationToken)
        {
            await Task.Delay(0);

            return new List<(long ShowId, long Updated)>()
            {
                ( ShowId: 1, Updated: 1549572248),
                ( ShowId: 2, Updated: 1551364282),
                ( ShowId: 3, Updated: 1534079818),
                ( ShowId: 4, Updated: 1554788133)
            };
        }
    }
}
