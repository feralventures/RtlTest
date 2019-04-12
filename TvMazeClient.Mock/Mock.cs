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

        public async Task<IDictionary<long, long>> GetUpdates(CancellationToken cancellationToken)
        {
            await Task.Delay(0);

            return new Dictionary<long, long>()
            {
                { 1, 1549572248 },
                { 2, 1551364282 },
                { 3, 1534079818 },
                { 4, 1554788133 }
            };
        }
    }
}
