using API_Shopping.Context;
using API_Shopping.DTOs.ShoppingCart;
using API_Shopping.Enums;
using API_Shopping.Exceptions.ShoppingCart;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _context;

        public ShoppingCartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetOrCreateShoppingCart(long userId)
        {
            if (userId <= 0)
                throw new InvalidUserIdException();

            var existingCart = await _context.ShoppingCarts
                .Include(c => c.ItemShoppingCarts)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == ShoppingCartStatus.Pending);

            if (existingCart != null)
                return existingCart;

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
            if (itemDto.Quantity <= 0)
                throw new InvalidQuantityException();

            var cart = await GetOrCreateShoppingCart(userId);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == itemDto.ProductId && p.IsActive == true)
                ?? throw new ProductNotFoundException(itemDto.ProductId);

            if (itemDto.Quantity > product.Quantity)
                throw new InsufficientStockException(product.Id, product.Quantity);

            var existingItem = await _context.ItemShoppingCarts
                .FirstOrDefaultAsync(i => i.shoppingCartId == cart.Id && i.productId == itemDto.ProductId);

            if (existingItem != null)
            {
                int newQuantity = existingItem.Quantity + itemDto.Quantity;
                if (newQuantity > product.Quantity)
                    throw new InsufficientStockException(product.Id, product.Quantity);

                existingItem.Quantity = newQuantity;
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

        public async Task<bool> UpdateProductItemFromCart(long itemId, int quantity, long userId)
        {
            if (quantity < 0)
                throw new InvalidQuantityException();

            var item = await _context.ItemShoppingCarts
                .Include(i => i.ShoppingCart)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.ShoppingCart.UserId == userId)
                ?? throw new CartItemNotFoundException(itemId);

            if (quantity == 0)
            {
                _context.ItemShoppingCarts.Remove(item);
            }
            else
            {
                if (quantity > item.Product.Quantity)
                    throw new InsufficientStockException(item.Product.Id, item.Product.Quantity);

                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductItemFromCart(long itemId, long userId)
        {
            var item = await _context.ItemShoppingCarts
                .Include(i => i.ShoppingCart)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.ShoppingCart.UserId == userId)
                ?? throw new CartItemNotFoundException(itemId);

            _context.ItemShoppingCarts.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}