using API_Shopping.DTOs.ShoppingCart;
using API_Shopping.Interfaces;
using API_Shopping.Models;
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

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        private long GetUserId() =>
            long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User identity not found."));

        // GET: api/shopping-cart
        [HttpGet]
        [Authorize(Roles = "client")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart()
        {
            var data = await _shoppingCartService.GetOrCreateShoppingCart(GetUserId());
            return Ok(data);
        }

        // POST: api/shopping-cart
        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<ActionResult<ShoppingCart>> AddItemIntoCart(ItemShoppingCartCreateDTO item)
        {
            var result = await _shoppingCartService.AddProductIntoCart(item, GetUserId());
            return CreatedAtAction(nameof(GetShoppingCart), result);
        }

        // PUT: api/shopping-cart/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> PutItemQuantity(long id, ItemShoppingCartUpdateDTO itemDto)
        {
            if (id != itemDto.Id)
                return BadRequest("Id mismatch.");

            await _shoppingCartService.UpdateProductItemFromCart(itemDto.Id, itemDto.Quantity, GetUserId());
            return NoContent();
        }

        // DELETE: api/shopping-cart/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteItemCart(long id)
        {
            await _shoppingCartService.DeleteProductItemFromCart(id, GetUserId());
            return NoContent();
        }
    }
}