using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TvMazeClient.Models;

namespace TvMazeClient
{
    public class Service : IService
    {
        readonly HttpClient _client;
        readonly string _baseUri;
        readonly SemaphoreSlim concurrentCalls = new SemaphoreSlim(1, 1);

        const HttpStatusCode TooManyRequests = ((HttpStatusCode)429);

        public Service(string baseUri)
        {
            _client = new HttpClient();
            _baseUri = baseUri;
        }

        private async Task<HttpResponseMessage> GetThrottledAsync(string requestUri, CancellationToken cancellationToken)
        {
            await concurrentCalls.WaitAsync();

            try
            {
                while (true)
                {
                    var response = await _client.GetAsync(requestUri, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }

                    if (response.StatusCode == TooManyRequests)
                    {
                        await Task.Delay(10000);

                        continue;
                    }

                    break;
                }
            }
            finally
            {
                concurrentCalls.Release();
            }

            throw new ApplicationException("Request to TVmaze.com failed. Please retry later.");
        }

        public async Task<IEnumerable<Actor>> GetCast(long showId, CancellationToken cancellationToken)
        {
            IList<Actor> actors = null;

            try
            {
                var response = await GetThrottledAsync($"{_baseUri}/shows/{showId}/cast", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    actors = JsonConvert.DeserializeObject<List<Actor>>(content);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return actors;
        }

        public async Task<IEnumerable<Show>> GetShows(long pageIndex, CancellationToken cancellationToken)
        {
            IList<Show> shows = null;

            try
            {
                var response = await GetThrottledAsync($"{_baseUri}/shows?page={pageIndex}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    shows = JsonConvert.DeserializeObject<List<Show>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return shows;
        }
    }
}
