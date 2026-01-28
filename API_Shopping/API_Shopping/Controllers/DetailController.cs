using API_Shopping.DTOs.Detail;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Shopping.Controllers
{
    [Route("api/details")]
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
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "client"
        )]
        public async Task<ActionResult<Product>> AddDetail(DetailCreateDTO[] detailDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            long userId = long.Parse(userIdClaim);

            var result = await _detailService.AddDetails(userId, detailDto);

            return CreatedAtAction(
                nameof(AddDetail),
                new { orderId = result.Id },
                result
            );
        }
    }
}
