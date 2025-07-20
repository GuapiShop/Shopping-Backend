using Microsoft.AspNetCore.Mvc;
using API_Shopping.Models;
using API_Shopping.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API_Shopping.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductsController : ControllerBase 
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductCreateDTO product)
        {
            Product result = await _productService.AddProduct(product);
            if ( result != null)
            {
                return CreatedAtAction("GetProduct", new { id = result.Id }, product);
            }
            else 
            { 
                return BadRequest();
            }   
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();

            if (products == null) {
                return NoContent(); //204
            } 
            else
            {
                return Ok(products);
            }
        }

        // GET: api/Products/number
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            if (await _productService.DeleteProduct(product))
            {
                return NoContent();
            }

            return StatusCode(500);
        } 
    }
}
