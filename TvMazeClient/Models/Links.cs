using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public class Links
    {
        [JsonProperty("self")]
        public Link Self { get; set; }

        [JsonProperty("previousepisode")]
        public Link PreviousEpisode { get; set; }

        [JsonProperty("nextepisode", NullValueHandling = NullValueHandling.Ignore)]
        public Link NextEpisode { get; set; }
    }
}
