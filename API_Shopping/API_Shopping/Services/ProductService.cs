using API_Shopping.Context;
using API_Shopping.DTOs.Product;
using API_Shopping.Exceptions.Product;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProduct(ProductCreateDTO productDto)
        {
            if (productDto == null)
                throw new ProductCreationException();

            var tempProduct = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                CodeCabys = productDto.CodeCabys,
                Price = productDto.Price,
                TaxCabys = productDto.TaxCabys,
                DescriptionCabys = productDto.DescriptionCabys,
                IsActive = true,
                CreateAt = DateTime.UtcNow
            };

            _context.Products.Add(tempProduct);
            await _context.SaveChangesAsync();

            return tempProduct;
        }

        // List products for admin
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
                    TaxCabys = u.TaxCabys,
                    DescriptionCabys = u.DescriptionCabys,
                    Quantity = u.Quantity,
                    IsActive = u.IsActive,
                    CreateAt = u.CreateAt,
                    UpdateAt = u.UpdateAt,
                })
                .ToListAsync();

            return new { page, totalPage, data = products };
        }

        // List products for client catalog
        public async Task<object> GetCatalogProducts(int page = 1, int pageSize = 10, string category = "")
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category && p.IsActive == true);

            var totalItems = await query.CountAsync();
            var totalPage = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Where(u => u.IsActive == true)
                .Select(u => new ProductResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Description = u.Description,
                    Category = u.Category,
                    Price = u.Price + (u.Price * u.TaxCabys),
                    Quantity = u.Quantity,
                })
                .ToListAsync();

            return new { page, totalPage, data = products };
        }

        public async Task<Product> GetProductById(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new ProductNotFoundException(id);

            return product;
        }

        public async Task<bool> UpdateProduct(long id, ProductUpdateDTO product)
        {
            var productFind = await GetProductById(id);

            try
            {
                productFind.Name = product.Name;
                productFind.Description = product.Description;
                productFind.Category = product.Category;
                productFind.Price = product.Price;
                productFind.TaxCabys = product.TaxCabys;
                productFind.CodeCabys = product.CodeCabys;
                productFind.DescriptionCabys = product.DescriptionCabys;
                productFind.UpdateAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ProductUpdateException(id);
            }

            return true;
        }

        public async Task<bool> DisableProduct(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new ProductNotFoundException(id);

            product.IsActive = false;
            product.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnableProduct(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new ProductNotFoundException(id);

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