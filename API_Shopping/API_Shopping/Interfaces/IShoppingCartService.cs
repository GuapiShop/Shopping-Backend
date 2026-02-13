using API_Shopping.DTOs.Product;
using API_Shopping.DTOs.ShoppingCart;
using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IShoppingCartService
    {
        public Task<ShoppingCart> GetOrCreateShoppingCart(long userId);
        public Task<ShoppingCart> AddProductIntoCart(ItemShoppingCartCreateDTO item, long userId);
        public Task<bool> DeleteProductItemFromCart(long itemId);
    }
}
