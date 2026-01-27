using API_Shopping.Interfaces;
using API_Shopping.Models;
using API_Shopping.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Shopping.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DetailController : ControllerBase
    {
        private readonly IDetailService _detailService;
        public DetailController(IDetailService detailService)
        {
            _detailService = detailService;
        }

        //POST: api/Detail
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> AddDetail(DetailCreateDTO[] detailDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            long userId = long.Parse(userIdClaim);

            Order result = await _detailService.AddDetails(userId, detailDto);
            if (result != null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("The detail could not be created");
            }
        }
    }
}
