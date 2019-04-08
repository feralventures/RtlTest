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

        private readonly Dictionary<long, Show[]> shows;
        private readonly Dictionary<long, Actor[]> cast;

        public Mock(string baseUri)
        {
            shows = new Dictionary<long, Show[]>()
            {
                { 0, GetResource<Show[]>("TvMazeClient.Mock.Resources.Shows.json") }
            };

            cast = new Dictionary<long, Actor[]>()
            {
                {  1, GetResource<Actor[]>("TvMazeClient.Mock.Resources.Shows_1_Cast.json") },
                {  2, GetResource<Actor[]>("TvMazeClient.Mock.Resources.Shows_2_Cast.json") }
            }; 
        }

        public async Task<IEnumerable<Actor>> GetCast(long showId, CancellationToken cancellationToken)
        {
            Actor[] actors;

            if (cast.TryGetValue(showId, out actors ))
            {
                return await Task.FromResult<Actor[]>(actors);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Show>> GetShows(long pageIndex, CancellationToken cancellationToken)
        {
            Show[] page;

            if (shows.TryGetValue(pageIndex, out page))
            {
                return await Task.FromResult<Show[]>(page);
            }
            else
            {
                return null;
            }
        }
    }
}
