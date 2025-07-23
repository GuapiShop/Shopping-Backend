using API_Shopping.Context;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context) {
            this._context = context;
        }

        //Add product
        public async Task<Product> AddProduct(ProductCreateDTO productDto)
        {
            if (productDto == null)
            {
                return null;
            }

            var tempProduct = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                CodeCABYS = productDto.CodeCABYS,
                Quantity = productDto.Quantity,
                Price = productDto.Price
            };
            _context.Products.Add(tempProduct);
            await _context.SaveChangesAsync();

            return tempProduct;
        }

        //List product
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //Get a single product
        public async Task<Product> GetProductById(long id)
        {
            return await _context.Products.FindAsync(id);
        }
        
        //Update product
        public async Task<bool> UpdateProduct(long id, Product product)
        {
            Product productFind = await GetProductById(id);
            try
            {
                if (productFind == null) return false;

                productFind.Name = product.Name;
                productFind.Description = product.Description;
                productFind.Category = product.Category;
                productFind.CodeCABYS = product.CodeCABYS;
                productFind.Quantity = product.Quantity;
                productFind.Price = product.Price;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        // delete product
        public async Task<bool> DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return false;
            }
            return true;
        }

        public async Task<bool> ProductExists(long id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}
