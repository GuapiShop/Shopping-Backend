using API_Shopping.Context;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace API_Shopping.Services
{
    public class ProductService: IProductService
    {
        private readonly AppDbContext _context;

        public ProductService( AppDbContext context) { 
            this._context = context;
        }

        //Add product
        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
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
            _context.Entry(product).State = EntityState.Modified;
            try
            {
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

        public bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
