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

        public async Task<ShoppingCart> GetOrCreateShoppingCart(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }

            var existingCart = await _context.ShoppingCarts
                .Include(c => c.ItemShoppingCarts)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Status == ShoppingCartStatus.Pending);

            if (existingCart != null)
            {
                return existingCart;
            }

            var newCart = new ShoppingCart
            {
                Status = ShoppingCartStatus.Pending,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };

            _context.ShoppingCarts.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        public async Task<ShoppingCart> AddProductIntoCart(ItemShoppingCartCreateDTO itemDto, long userId)
        {
            var cart = await GetOrCreateShoppingCart(userId);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == itemDto.ProductId);

            if (product != null)
            {
                new Exception("Product not found");
            }
                
            var existingItem = await _context.ItemShoppingCarts
                .FirstOrDefaultAsync(i => i.shoppingCartId == cart.Id && i.productId == itemDto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
            }
            else
            {
                var newItem = new ItemShoppingCart
                {
                    productId = itemDto.ProductId,
                    UnitPrice = itemDto.UnitPrice,
                    Quantity = itemDto.Quantity,
                    shoppingCartId = cart.Id,
                };

                _context.ItemShoppingCarts.Add(newItem);
            }

            await _context.SaveChangesAsync();

            return await GetOrCreateShoppingCart(userId);
        }

        public async Task<bool> DeleteProductItemFromCart(long itemId, long userId)
        {
            var item = await _context.ItemShoppingCarts
                .Include(i=>i.ShoppingCart)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.ShoppingCart.UserId == userId);

            if (item == null)
                return false;

            _context.ItemShoppingCarts.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProductItemFromCart(long itemId, int quantity, long userId)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative");

            var item = await _context.ItemShoppingCarts
                .Include(i => i.ShoppingCart)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.ShoppingCart.UserId == userId);

            if (item == null)
                return false;

            if (quantity <= 0)
            {
                _context.ItemShoppingCarts.Remove(item);
            }
            else
            {
                if (quantity > item.Product.Quantity)
                    throw new Exception("Not enough stock");

                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
