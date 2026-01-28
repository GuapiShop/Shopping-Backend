using API_Shopping.Context;
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

        public async Task<Order> AddDetails(long userId, DetailCreateDTO[] detailsDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var user = _context.Users.FindAsync(userId);

            var order = new Order
            {
                UserId = userId,
                State = "",
                CreateAt = DateTime.Now,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in detailsDto) 
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null) continue;

                var detail = new Detail
                {
                    ProductId = product.Id, //not value
                    OrderId = order.Id.Value, //not value
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Total = item.Quantity * product.Price,
                };
                _context.Details.Add(detail);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return order;
        }
    }
}
