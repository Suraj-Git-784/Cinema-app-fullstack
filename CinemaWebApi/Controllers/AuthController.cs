using Cinema.BLL.Interface;
using Cinema.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginUser(LoginModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }
                var result = await authRepository.Login(model);

                if (result is OkObjectResult okResult)
                {
                    return okResult;
                }
                else if (result is UnauthorizedResult unauthorizedResult)
                {
                    return unauthorizedResult;
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Invalid Credentials");
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel mdoel)
        {
            try
            {
                if(mdoel == null)
                {
                    return BadRequest();
                }
                var user = await authRepository.Register(mdoel);
                return NoContent();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new user record");
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterUserAsAdmin(RegisterModel model)
        {
            try
            {
                if(model == null)
                {
                    return BadRequest();
                }
                var user = await authRepository.RegisterAdmin(model);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating admin");
            }
        }
    }   
}
