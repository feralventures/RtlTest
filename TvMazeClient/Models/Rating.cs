using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public class Rating
    {
        [JsonProperty("average")]
        public double? Average { get; set; }
    }
}
