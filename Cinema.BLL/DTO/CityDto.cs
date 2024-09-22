namespace Cinema.BLL.DTO
{
    public class CityDto
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public List<TheaterDetailDto> Theaters { get; set; }
    }



    public class TheaterDetailDto
    {
        public int TheaterId { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public List<ShowTimingDto> ShowTimings { get; set; }
    }

    public class ShowTimingDto
    {
        public int ShowTimingId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public DateTime ShowTime { get; set; }
        public MovieDetailsDto Movie { get; set; }
        public string Theater { get; set; }
    }

    public class MovieDetailsDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> ShowTimings { get; set; }
    }
}
