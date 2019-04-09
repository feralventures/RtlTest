using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeClient.Models
{
    public partial class Embedded
    {
        [JsonProperty("cast")]
        public IList<Actor> Cast { get; set; }
    }
}
