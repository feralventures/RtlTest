using System;
using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public partial class Image
    {
        [JsonProperty("medium")]
        public Uri Medium { get; set; }

        [JsonProperty("original")]
        public Uri Original { get; set; }
    }
}
