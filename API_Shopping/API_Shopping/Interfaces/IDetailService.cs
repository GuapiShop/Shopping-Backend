using API_Shopping.DTOs.Detail;
using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IDetailService
    {
        public Task<Order> AddDetails(long userId, DetailCreateDTO[] details);
    }
}
