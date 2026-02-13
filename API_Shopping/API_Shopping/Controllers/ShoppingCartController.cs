using API_Shopping.DTOs.Product;
using API_Shopping.DTOs.ShoppingCart;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using API_Shopping.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Shopping.Controllers
{
    [Route("api/shopping-cart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService) {
            this._shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "client"
        )]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            long userId = long.Parse(userIdClaim);

            var data = await _shoppingCartService.GetOrCreateShoppingCart(userId);

            return Ok(data);
        }

        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "client"
        )]
        public async Task<ActionResult<ShoppingCart>> AddItemIntoCart(ItemShoppingCartCreateDTO item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            long userId = long.Parse(userIdClaim);

            var result = await _shoppingCartService.AddProductIntoCart(item, userId);

            return CreatedAtAction(
                nameof(GetShoppingCart),
                new { userId = userId },
                result
            );
        }
    }
}
