using API_Shopping.Models;
using System.Diagnostics.Eventing.Reader;

namespace API_Shopping.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetProductById(long id);
        public Task<Product> AddProduct(Product product);
        public Task<bool> UpdateProduct(long id, Product product);
        public Task<bool> DeleteProduct(Product product);
        public bool ProductExists(long id);
    }
}
