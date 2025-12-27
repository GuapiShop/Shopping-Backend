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
                CodeCabys = productDto.CodeCabys,
                Price = productDto.Price,
                IsActive = true,
                CreateAt = DateTime.UtcNow
            };
            _context.Products.Add(tempProduct);
            await _context.SaveChangesAsync();

            return tempProduct;
        }

        //List product
        public async Task<object> GetProducts(int page = 1, int pageSize = 10)
        {
            var query = _context.Products.AsQueryable();
            var totalItems = await query.CountAsync();
            var totalPage = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new Product
                {
                    Id = u.Id,
                    Name = u.Name,
                    Description = u.Description,
                    Category = u.Category,
                    CodeCabys = u.CodeCabys,
                    Price = u.Price,
                    IsActive = u.IsActive,
                    CreateAt = u.CreateAt,
                    UpdateAt = u.UpdateAt,
                })
                .ToListAsync();

            return new
            {
                page,
                totalPage,
                data = products
            };
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
                productFind.CodeCabys = product.CodeCabys;
                productFind.Price = product.Price;
                productFind.IsActive = product.IsActive;
                productFind.UpdateAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        // disable product
        public async Task<bool> DisableProduct(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync( p => p.Id == id);

            if (product==null)
                return false;

            product.IsActive = false;
            product.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        // enable product
        public async Task<bool> EnableProduct(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            product.IsActive = true;
            product.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProductExists(long id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}
