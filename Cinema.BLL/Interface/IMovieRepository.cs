using Cinema.BLL.DTO;
using Cinema.DAL.Models;

namespace Cinema.BLL.Interface
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieDto>> GetMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<Movie> AddMovieAsync(Movie movie);
        Task<Movie> UpdaetMovieAsync(Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
