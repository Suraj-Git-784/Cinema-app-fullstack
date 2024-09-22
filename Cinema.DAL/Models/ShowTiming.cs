namespace Cinema.DAL.Models
{
    public class ShowTiming
    {
        public int ShowTimingId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public DateTime ShowTime { get; set; }

        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
    }
}
