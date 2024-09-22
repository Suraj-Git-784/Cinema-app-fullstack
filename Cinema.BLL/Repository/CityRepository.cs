using Cinema.BLL.DTO;
using Cinema.BLL.Interface;
using Cinema.DAL;
using Cinema.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.BLL.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext context;

        public CityRepository(AppDbContext context)
        {
            this.context = context;


        }

        public async Task<string> CreateCityAsync(CreateCityDto city)
        {
            var existingCity = await context.Cities.FirstOrDefaultAsync(c => c.Name == city.Name);
            if (existingCity != null)
            {
                return "City name already exists";
            }

            // Create the new city
            var data = new City
            {
                Name = city.Name,
            };

            // Add the new city to the database
            await context.Cities.AddAsync(data);
            await context.SaveChangesAsync();

            return $"City '{data.Name}' created successfully";
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            var data = await context.Cities.ToListAsync();
            return data;
        }

        public async Task<CityDto> GetCityByIdAsync(int id)
        {
            var city = await context.Cities
                                .Include(c => c.Theaters)
                                .ThenInclude(t => t.ShowTimings)
                                .FirstOrDefaultAsync(c => c.CityId == id);

            if (city == null)
            {
                return null;
            }

            var cityDto = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                Theaters = city.Theaters.Select(t => new TheaterDetailDto
                {
                    TheaterId = t.TheaterId,
                    Name = t.Name,
                    CityId = t.CityId,
                    City = t.City.Name,
                    ShowTimings = t.ShowTimings?.Select(st => new ShowTimingDto
                    {
                        ShowTimingId = st.ShowTimingId,
                        MovieId = st.MovieId,
                        TheaterId = st.TheaterId,
                        ShowTime = st.ShowTime,
                        Movie = st.Movie != null ? new MovieDetailsDto
                        {
                            MovieId = st.Movie.MovieId,
                            Title = st.Movie.Title,
                            Description = st.Movie.Description,
                            Genre = st.Movie.Genre,
                            ReleaseDate = st.Movie.ReleaseDate,
                            ShowTimings = st.Movie.ShowTimings?.Select(mst => mst.ToString()).ToList()
                        } : null,
                        Theater = t.Name
                    }).ToList()
                }).ToList()
            };

            return cityDto;
        }
    }
}
