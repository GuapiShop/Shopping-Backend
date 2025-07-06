using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Shopping.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            if (users == null) 
            { 
                return NoContent();
            }else
            {
                return Ok(users);
            }  
        }

        // GET: api/Users/number
        [HttpGet("{id}")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UpdateUserDTO user)
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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
