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

        static readonly SemaphoreSlim concurrentCalls = new SemaphoreSlim(1, 1);

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
                HttpResponseMessage result = null;

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        Debug.WriteLine($"GET {requestUri}");

                        result = await _client.GetAsync(requestUri, cancellationToken);

                        if (result.StatusCode == TooManyRequests)
                        {
                            var millisecondsDelay = 10000;

                            Debug.WriteLine($"Delaying next GET to endpoint with {millisecondsDelay}ms.");
                            await Task.Delay(millisecondsDelay);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"Request {requestUri} failed.", ex);
                    }
                }

                return result;
            }
            finally
            {
                concurrentCalls.Release();
            }
        }

        public async Task<Show> GetShow(long showId, CancellationToken cancellationToken)
        {
            var response = await GetThrottledAsync($"{_baseUri}/shows/{showId}?embed[]=cast", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var show = JsonConvert.DeserializeObject<Show>(content);

                return show;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<(long ShowId, long Updated)>> GetUpdates(CancellationToken cancellationToken)
        {
            var result = new List<(long ShowId, long Updated)>();

            var response = await GetThrottledAsync($"{_baseUri}/updates/shows", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserialized = JsonConvert.DeserializeObject<Dictionary<string, long>>(content);

                foreach (var item in deserialized)
                {
                    result.Add((ShowId: long.Parse(item.Key), Updated: item.Value));
                }
            }
            else
            {
            }

            return result;
        }
    }
}
