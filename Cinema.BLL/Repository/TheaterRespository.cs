using Cinema.BLL.Interface;
using Cinema.DAL;
using Cinema.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.BLL.Repository
{
    public class TheaterRespository : ITheaterRepository
    {
        private readonly AppDbContext context;

        public TheaterRespository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Theater>> GetAllTheatersAsync(int cityId)
         {
            var data = await context.Theaters
                          .Include(t => t.City) // Eagerly load the related City entity
                          .Where(t => t.CityId == cityId)
                          .ToListAsync(); // Execute the query asynchronously

            return data;
        }
    }
}
