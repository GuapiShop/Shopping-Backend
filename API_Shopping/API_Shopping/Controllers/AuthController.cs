using API_Shopping.DTOs.Auth;
using API_Shopping.Exceptions.Auth;
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

            if (login.Email == null || login.Email == "") {
                throw new EmptyEmailException();
            }

            if (login.Password == null || login.Password == "") { 
                throw new EmptyPasswordException();
            }

            User findUser = await _jwtService.FindUserWithLogin(login);

            if (findUser == null)
            {
                throw new InvalidCredentialsException();
            }
            return Ok(new { message = "User found", token = _jwtService.JWTGenerator(findUser), role = findUser.Role});
        }
    }
}
