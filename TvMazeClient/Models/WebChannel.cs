using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public class WebChannel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }
    }
}
