using API_Shopping.Context;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<Product>> getProducts()
        {
            return await _context.Products.ToListAsync();
        }




        public Task<bool> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        

        public Task<bool> UpdateProduct(int id, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
