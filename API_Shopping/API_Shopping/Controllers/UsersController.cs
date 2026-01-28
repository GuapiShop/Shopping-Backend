using API_Shopping.DTOs.User;
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

        // GET: api/Users
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<ActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            var data = await _userService.GetUsers(page, pageSize);
            if (data == null) 
            { 
                return NoContent();
            }else
            {
                return Ok(data);
            }  
        }

        // GET: api/Users/number
        [HttpGet("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/number
        [HttpPut("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<IActionResult> PutUser(long id, UserUpdateDTO user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!await _userService.UserExists(id))
            {
                return NotFound("User not found");
            }

            if (await _userService.UpdateUser(id, user))
            { 
                return NoContent();
            }
            
            return StatusCode(500);
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(UserCreateDTO user)
        {
            User result = await _userService.AddUser(user);
            if (result != null)
            {
                return CreatedAtAction("GetUser", new { id = result.Id }, user);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: api/Users/disable/number
        [HttpPut("disable/{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<IActionResult> DisableUser(long id)
        {
            if (!await _userService.UserExists(id))
            {
                return NotFound("User not found");
            }

            if (await _userService.DisableUser(id)) 
            {
                return NoContent();
            }

            return StatusCode(500);
        }

        // PUT: api/Users/enable/number
        [HttpPut("enable/{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<IActionResult> EnableUser(long id)
        {
            if (!await _userService.UserExists(id))
            {
                return NotFound("User not found");
            }

            if (await _userService.EnableUser(id))
            {
                return NoContent();
            }

            return StatusCode(500);
        }
    }
}
