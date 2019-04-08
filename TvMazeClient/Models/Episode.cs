using System;
using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public class Link
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
    }
}
