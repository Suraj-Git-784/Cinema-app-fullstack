using Cinema.BLL.DTO;
using Cinema.BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<List<MovieDto>>> GetAllMovies()
        {
            try
            {
               var data = await movieRepository.GetMoviesAsync();
                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
               "Error something went wrong");
            }
        }
    }
}
