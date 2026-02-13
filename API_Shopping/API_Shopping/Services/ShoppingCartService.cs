using API_Shopping.Context;
using API_Shopping.DTOs.ShoppingCart;
using API_Shopping.Enums;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _context;

        public ShoppingCartService(AppDbContext context) { 
            this._context = context;
        }

        public async Task<ItemShoppingCart> AddProductIntoCart(ItemShoppingCartCreateDTO[] items)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetOrCreateShoppingCart(long userId)
        {
            if (userId <= 0) {
                throw new ArgumentException("Invalid user id");
            }

            var existingCart = await _context.ShoppingCarts
                .Include(c => c.ItemShoppingCarts)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Status == ShoppingCartStatus.Pending);

            if (existingCart != null) {
                return existingCart;
            }

            var newCart = new ShoppingCart {
               Status = ShoppingCartStatus.Pending,
               UserId = userId,
               CreatedAt = DateTime.UtcNow,
            };

            _context.ShoppingCarts.Add(newCart);
            await _context.SaveChangesAsync();
            
            return newCart;
        }
    }
}
