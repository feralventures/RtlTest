using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public class Externals
    {
        [JsonProperty("tvrage")]
        public string TVRage { get; set; }

        [JsonProperty("thetvdb")]
        public string TheTVDB { get; set; }

        [JsonProperty("imdb")]
        public string IMDb { get; set; }
    }
}
