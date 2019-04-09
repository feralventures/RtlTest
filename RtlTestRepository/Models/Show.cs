using System.Collections.Generic;

namespace RtlTestRepository.Models
{
    public class Show
    {
        public long Id { get; set; }
        public long TvMazeId { get; set; }
        public long Updated { get; set; }
        public string Name { get; set; }
        public List<Person> Cast { get; set; }
    }
}
