using API_Shopping.Context;
using API_Shopping.DTOs.Detail;
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

        public async Task<Order> AddDetails(long userId, DetailCreateDTO[] detailsDto)
        {
            if (detailsDto == null || detailsDto.Length == 0)
                throw new DetailEmptyException();

            // Validate each item quantity before touching the DB
            foreach (var item in detailsDto)
            {
                if (item.Quantity <= 0)
                    throw new InvalidQuantityException();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            var user = await _context.Users.FindAsync(userId)
                ?? throw new UserNotFoundException(userId);

            var order = new Order
            {
                UserId = userId,
                State = "pending",
                CreateAt = DateTime.UtcNow,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in detailsDto)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId && p.IsActive == true)
                    ?? throw new ProductNotFoundException(item.ProductId);

                if (product.Quantity < item.Quantity)
                    throw new InsufficientStockException(product.Id, product.Quantity);

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