namespace RtlTestRepository.Models
{
    public class Person
    {
        public long Id { get; set; }
        public long TvMazeId { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public long ShowId { get; set; }
        public Show Show { get; set; }
    }
}
