using API_Shopping.DTOs.User;
using API_Shopping.Exceptions.User;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Shopping.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            var data = await _userService.GetUsers(page, pageSize);
            return Ok(data);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> PutUser(long id, UserUpdateDTO user)
        {
            if (id != user.Id)
                throw new UserIdMismatchException();

            await _userService.UpdateUser(id, user);
            return NoContent();
        }

        // POST: api/users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(UserCreateDTO user)
        {
            User result = await _userService.AddUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
        }

        // PUT: api/users/disable/{id}
        [HttpPut("disable/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DisableUser(long id)
        {
            await _userService.DisableUser(id);
            return NoContent();
        }

        // PUT: api/users/enable/{id}
        [HttpPut("enable/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> EnableUser(long id)
        {
            await _userService.EnableUser(id);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}