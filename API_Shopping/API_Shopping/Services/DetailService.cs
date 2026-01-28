using API_Shopping.Context;
using API_Shopping.DTOs.Detail;
using API_Shopping.Exceptions;
using API_Shopping.Interfaces;
using API_Shopping.Models;

namespace API_Shopping.Services
{
    public class DetailService : IDetailService
    {
        private readonly AppDbContext _context;

        public DetailService( AppDbContext context) {
            this._context = context;
        }

        // Add and build detail
        public async Task<Order> AddDetails(long userId, DetailCreateDTO[] detailsDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var user = _context.Users.FindAsync(userId);

            var order = new Order
            {
                UserId = userId,
                State = "pending",
                CreateAt = DateTime.Now,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in detailsDto) 
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null) 
                {
                    throw new ProductNotFoundException($"Product don't exist.");
                }

                if (product.Quantity < item.Quantity) 
                {
                    throw new OutOfStockException($"Stock insuficiente para {product.Name}");
                }

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
    }
}
