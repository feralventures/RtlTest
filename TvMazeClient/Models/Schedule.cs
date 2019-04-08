using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public class Schedule
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("days")]
        public IList<string> Days { get; set; }
    }
}
