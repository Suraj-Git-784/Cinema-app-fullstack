namespace Cinema.DAL.Models
{
    public class Theater
    {
        public int TheaterId { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }

        public City City { get; set; }
        public ICollection<ShowTiming> ShowTimings { get; set; }
    }
}
