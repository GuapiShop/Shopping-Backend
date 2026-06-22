using API_Shopping.DTOs.Product;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Shopping.Controllers
{
    [Route("api/product")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: api/product
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<Product>> AddProduct(ProductCreateDTO product)
        {
            var result = await _productService.AddProduct(product);
            return CreatedAtAction("GetProduct", new { id = result.Id }, result);
        }

        // GET: api/product
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            var data = await _productService.GetProducts(page, pageSize);
            return Ok(data);
        }

        // GET: api/product/catalog
        [HttpGet("catalog")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "client")]
        public async Task<ActionResult<ProductResponseDTO>> GetShowProducts(int page = 1, int pageSize = 10, string category = "")
        {
            var data = await _productService.GetCatalogProducts(page, pageSize, category);
            return Ok(data);
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }

        // PUT: api/product/{id}
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> PutProduct(long id, ProductUpdateDTO product)
        {
            if (id != product.Id)
                return BadRequest("The ID in the URL does not match the ID in the request body.");

            await _productService.UpdateProduct(id, product);
            return NoContent();
        }

        // PUT: api/product/disable/{id}
        [HttpPut("disable/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DisableProduct(long id)
        {
            await _productService.DisableProduct(id);
            return NoContent();
        }

        // PUT: api/product/enable/{id}
        [HttpPut("enable/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> EnableProduct(long id)
        {
            await _productService.EnableProduct(id);
            return NoContent();
        }
    }
}