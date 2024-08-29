using Microsoft.AspNetCore.Mvc;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;

namespace RentalManager.WebAPI.Controllers
{
    public class ProductDto
    {
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string TypeName { get; set; } = null!;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRentalManagerUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IRentalManagerUnitOfWork unitOfWork, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("Getting all products.");
            var items = await _unitOfWork.ProductRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"Getting product with id: {id}.");
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                _logger.LogInformation("Product not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            try
            {
                _logger.LogInformation($"Updating product with id: {id}.");
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogInformation("Product not found.");
                    return NotFound("Product not found");
                }
                var productType = await _unitOfWork.ProductTypeRepository.GetProductTypeByNameAsync(productDto.TypeName);
                if (productType == null)
                {
                    _logger.LogInformation("Product type not found.");
                    return NotFound("Product type not found");
                }

                product.ProductName = productDto.ProductName;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.QuantityInStock = productDto.Quantity;
                product.ProductTypeId = productType.ProductTypeId;
                product.ProductType = productType;
                await _unitOfWork.ProductRepository.UpdateAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductDto product)
        {
            try
            {
                _logger.LogInformation("Adding new product.");
                var type = await _unitOfWork.ProductTypeRepository.GetProductTypeByNameAsync(product.TypeName);
                if (type == null)
                {
                    _logger.LogInformation("Product type not found.");
                    return NotFound("Product type not found");
                }
                var item = new Product
                {
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    QuantityInStock = product.Quantity,
                    ProductTypeId = type.ProductTypeId,
                    ProductType = type
                };

                await _unitOfWork.ProductRepository.InsertAsync(item);
                _unitOfWork.Commit();

                return CreatedAtAction("GetProductById", new { id = item.ProductId }, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new product.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Deleting product with id: {id}.");
            await _unitOfWork.ProductRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}