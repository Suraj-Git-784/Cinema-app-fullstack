using Cinema.BLL.DTO;
using Cinema.DAL.Models;

namespace Cinema.BLL.Interface
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        //Task<IEnumerable<TheaterDto>> GetTheatersByCityIdAsync(int cityId);
        Task<CityDto> GetCityByIdAsync(int id);
        Task<string> CreateCityAsync(CreateCityDto city);
    }
}
