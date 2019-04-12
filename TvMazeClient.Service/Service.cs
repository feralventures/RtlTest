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

        public string BaseUri { get; set; }
        public int TooManyRequestsDelay { get; set; }

        static readonly SemaphoreSlim concurrentCalls = new SemaphoreSlim(1, 1);

        const HttpStatusCode TooManyRequests = ((HttpStatusCode)429);

        public Service()
        {
            _client = new HttpClient();
        }

        private async Task<HttpResponseMessage> GetThrottledAsync(string relativeUri, CancellationToken cancellationToken)
        {
            var requestUri = $"{BaseUri}/{relativeUri}";

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
                            var millisecondsDelay = TooManyRequestsDelay;

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
            var response = await GetThrottledAsync($"shows/{showId}?embed[]=cast", cancellationToken);

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

        public async Task<IDictionary<long, long>> GetUpdates(CancellationToken cancellationToken)
        {
            Dictionary<long, long> result = null;

            var response = await GetThrottledAsync($"updates/shows", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<Dictionary<long, long>>(content);
            }
            else
            {
            }

            return result;
        }
    }
}
