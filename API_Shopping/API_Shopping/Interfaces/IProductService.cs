using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IProductService
    {
        public Task<object> GetProducts(int page, int pageSize);
        public Task<object> GetShowProducts(int page, int pageSize, string category);
        public Task<Product> GetProductById(long id);
        public Task<Product> AddProduct(ProductCreateDTO product);
        public Task<bool> UpdateProduct(long id, ProductUpdateDTO product);
        public Task<bool> DisableProduct(long id);
        public Task<bool> EnableProduct(long id);
        public Task<bool> ProductExists(long id);
    }
}
