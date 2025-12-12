using API_Shopping.Models;
using API_Shopping.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Shopping.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService) {
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login(LoginDTO login) {
            User findUser = await _jwtService.FindUserWithLogin(login);

            if (findUser == null)
            {
                return NotFound("Bad credentials");
            }
            return Ok(new { message = "User found", token = _jwtService.JWTGenerator(findUser), role = findUser.Role});
        }
    }
}
