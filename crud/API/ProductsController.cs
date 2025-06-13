using crud.Domain;
using crud.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using crud.Domain;
using crud.Infrastructure;// Assuming you have a DTO namespace for ProductDto

namespace crud.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDto productDto)
        {
            // Convert DTO to Product model
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                CreatedAt = DateTime.Now // Set creation date on the backend
            };

            var addedProduct = await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = addedProduct.Id }, addedProduct);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;

            var updated = await _productRepository.UpdateAsync(existingProduct);
            if (!updated)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent(); // 204 No Content for successful update
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
            {
                return StatusCode(500, "A problem happened while deleting the product.");
            }
            return NoContent(); // 204 No Content for successful deletion
        }
    }
}
