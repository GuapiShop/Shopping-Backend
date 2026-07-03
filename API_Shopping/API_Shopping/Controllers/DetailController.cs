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

        private long GetUserId() =>
            long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User identity not found."));

        // POST: api/details
        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<ActionResult<Order>> AddDetail(DetailCreateDTO[] detailDto)
        {
            var result = await _detailService.AddDetails(GetUserId(), detailDto);
            return CreatedAtAction(nameof(AddDetail), new { id = result.Id }, result);
        }

        // GET: api/details/pending
        [HttpGet("pending")]
        [Authorize(Roles = "client")]
        public async Task<ActionResult<List<PendingOrderDTO>>> GetPendingOrders()
        {
            var result = await _detailService.GetPendingOrders(GetUserId());
            return Ok(result);
        }
    }
}