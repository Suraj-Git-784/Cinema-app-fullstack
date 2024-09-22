using Cinema.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.BLL.Interface
{
    public interface IAuthRepository
    {
        Task<ActionResult> Login(LoginModel model);
        Task<IActionResult> Register(RegisterModel model);
        Task<IActionResult> RegisterAdmin(RegisterModel model);
    }
}
