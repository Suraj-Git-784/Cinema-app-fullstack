namespace Cinema.DAL.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public ICollection<Theater> Theaters { get; set; }
    }
    public class CreateCityDto
    {
        public string Name { get; set; }
    }
}
