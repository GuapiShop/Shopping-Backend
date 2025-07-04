using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetProductById(int id);
        public Task<Product> AddProduct(Product product);
        public Task<bool> UpdateProduct(int id, Product product);
        public Task<bool> DeleteProduct(int id);
    }
}
