using Cinema.BLL.DTO;
using Cinema.BLL.Interface;
using Cinema.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : Controller
    {
        private readonly ICityRepository cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCityActionResult()
        {
            var data = await cityRepository.GetCitiesAsync();
            return Ok(data);
        }

        // Get city by id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CityDto>> GetCityById(int id)
        {
            var data = await cityRepository.GetCityByIdAsync(id);
            return Ok(data);
        }

        // Create city to the list
        [HttpPost]
        [Authorize(Roles =UserRoles.Admin)]
        public async Task<ActionResult> CreateCityAsync([FromBody] CreateCityDto City)
        {
            var result = await cityRepository.CreateCityAsync(City);

            if (result == "City name already exists")
            {
                return Conflict();  // HTTP 409 Conflict
            }

            return NoContent();
        }
    }
}
