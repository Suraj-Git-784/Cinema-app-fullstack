using Cinema.DAL.Models;

namespace Cinema.BLL.Interface
{
    public interface ITheaterRepository
    {
        Task<List<Theater>> GetAllTheatersAsync(int cityId);
    }
}
