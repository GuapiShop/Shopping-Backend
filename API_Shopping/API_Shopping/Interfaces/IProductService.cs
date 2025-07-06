using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetProductById(long id);
        public Task<Product> AddProduct(ProductCreateDTO product);
        public Task<bool> UpdateProduct(long id, Product product);
        public Task<bool> DeleteProduct(Product product);
        public Task<bool> ProductExists(long id);
    }
}
