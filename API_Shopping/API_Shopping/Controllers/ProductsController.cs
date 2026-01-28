using Microsoft.AspNetCore.Mvc;
using API_Shopping.Models;
using API_Shopping.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API_Shopping.DTOs.Product;

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

        // POST: api/products
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<ActionResult<Product>> AddProduct(ProductCreateDTO product)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Product result = await _productService.AddProduct(product);

            if ( result != null)
            {
                return CreatedAtAction("GetProduct", new { id = result.Id }, result);
            }
            else 
            { 
                return BadRequest("The product could not be created. Please try again later.");
            }   
        }

        // GET: api/products
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<ActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            var data = await _productService.GetProducts(page, pageSize);

            if (data == null) {
                return NoContent();
            } 
            else
            {
                return Ok(data);
            }
        }

        // GET: api/products/catalog
        [HttpGet("catalog")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "client"
        )]
        public async Task<ActionResult<ProductResponseDTO>> GetShowProducts(int page = 1, int pageSize = 10, string category="") 
        {
            var data = await _productService.GetCatalogProducts(page, pageSize, category);
            if (data == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(data);
            }
        }

        // GET: api/products/number
        [HttpGet("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, 
            Roles = "admin,client"
        )]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<IActionResult> PutProduct(long id, ProductUpdateDTO product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (await _productService.UpdateProduct(id, product)) {
                return NoContent();
            }

            if (!await _productService.ProductExists(id))
            {
                return NotFound();
            }
            return StatusCode(500);
        }

        // PUT: api/products/disable/5
        [HttpPut("disable/{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<IActionResult> DisableProduct(long id)
        {
            if (!await _productService.ProductExists(id))
            {
                return NotFound("Product not found");
            }

            if (await _productService.DisableProduct(id))
            {
                return NoContent();
            }

            return StatusCode(500);
        }

        // PUT: api/products/enable/5
        [HttpPut("enable/{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "admin"
        )]
        public async Task<IActionResult> EnableProduct(long id)
        {
            if (!await _productService.ProductExists(id))
            {
                return NotFound("Product not found");
            }

            if (await _productService.EnableProduct(id))
            {
                return NoContent();
            }

            return StatusCode(500);
        }
    }
}
