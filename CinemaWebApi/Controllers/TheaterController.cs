using Cinema.BLL.Interface;
using Cinema.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CinemaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterRepository theaterRespository;

        public TheaterController(ITheaterRepository theaterRespository)
        {
            this.theaterRespository = theaterRespository;
        }

        [HttpGet("{cityId}")]
        [Authorize]
        public async Task<ActionResult<List<Theater>>> GetResultTheaterAsync([FromRoute] int cityId)
        {
            var data = await theaterRespository.GetAllTheatersAsync(cityId);
            return Ok(data);
        }
    }
}
