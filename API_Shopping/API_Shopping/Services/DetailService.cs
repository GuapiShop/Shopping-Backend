using API_Shopping.Context;
using API_Shopping.DTOs.Detail;
using API_Shopping.Enums;
using API_Shopping.Exceptions.Detail;
using API_Shopping.Exceptions.Product;
using API_Shopping.Exceptions.ShoppingCart;
using API_Shopping.Exceptions.User;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class DetailService : IDetailService
    {
        private readonly AppDbContext _context;

        public DetailService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddDetails(long userId)
        {
            var user = await _context.Users.FindAsync(userId)
                ?? throw new UserNotFoundException(userId);

            var cart = await _context.ShoppingCarts
                .Include(c => c.ItemShoppingCarts)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == ShoppingCartStatus.Pending)
                ?? throw new CartItemNotFoundException(userId);

            if (!cart.ItemShoppingCarts.Any())
                throw new EmptyCartException();

            using var transaction = await _context.Database.BeginTransactionAsync();

            var order = new Order
            {
                UserId = userId,
                State = "pending",
                CreateAt = DateTime.UtcNow,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart.ItemShoppingCarts)
            {
                var product = item.Product;

                if (product == null || !product.IsActive)
                    throw new ProductNotFoundException(item.productId);

                if (product.Quantity < item.Quantity)
                    throw new OutOfStockException("Out of stock");

                product.Quantity -= item.Quantity;

                var detail = new Detail
                {
                    ProductId = product.Id,
                    OrderId = order.Id,
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Total = item.Quantity * product.Price,
                };

                _context.Details.Add(detail);
                _context.Products.Update(product);
            }

            _context.ItemShoppingCarts.RemoveRange(cart.ItemShoppingCarts);
            _context.ShoppingCarts.Remove(cart);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }

        public async Task<List<PendingOrderDTO>> GetPendingOrders(long userId)
        {
            var user = await _context.Users.FindAsync(userId)
                ?? throw new UserNotFoundException(userId);

            var orders = await _context.Orders
                .Where(o => o.UserId == userId && o.State == "pending")
                .Select(o => new PendingOrderDTO
                {
                    OrderId = o.Id,
                    State = o.State,
                    CreatedAt = o.CreateAt,
                    Items = _context.Details
                        .Where(d => d.OrderId == o.Id)
                        .Select(d => new DetailProductDTO
                        {
                            ProductId = d.ProductId,
                            ProductName = d.Product.Name,
                            Category = d.Product.Category,
                            Quantity = d.Quantity,
                            UnitPrice = d.Price,
                            Total = d.Total,
                        })
                        .ToList(),
                    GrandTotal = _context.Details
                        .Where(d => d.OrderId == o.Id)
                        .Sum(d => d.Total),
                })
                .ToListAsync();

            if (!orders.Any())
                throw new DetailOrderNotFoundException(userId);

            return orders;
        }
    }
}