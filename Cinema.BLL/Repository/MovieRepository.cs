using Cinema.BLL.DTO;
using Cinema.BLL.Interface;
using Cinema.DAL;
using Cinema.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.BLL.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext context;

        public MovieRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Task<Movie> AddMovieAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovieAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesAsync()
        {

            return await context.Movies
                .Select(s => new MovieDto
                {
                    MovieId = s.MovieId,
                    MovieName = s.Title,
                    MovieDescription = s.Description,
                    MovieGenre = s.Genre
                }).ToListAsync();
        }

        public Task<Movie> UpdaetMovieAsync(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
