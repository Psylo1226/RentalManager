using Microsoft.AspNetCore.Mvc;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;

namespace RentalManager.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductTypeController :ControllerBase
{
    private readonly IRentalManagerUnitOfWork _unitOfWork;
    private readonly ILogger<ProductTypeController> _logger;

    public ProductTypeController(IRentalManagerUnitOfWork unitOfWork, ILogger<ProductTypeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProductTypes()
    {
        _logger.LogInformation("Getting all product types.");
        var items = await _unitOfWork.ProductTypeRepository.GetAllAsync();
        return Ok(items);
    }
    [HttpGet("/name/{name}")]
    public async Task<IActionResult> GetProductTypeByName(string name)
    {
        _logger.LogInformation($"Getting product type with name: {name}.");
        var productType = await _unitOfWork.ProductTypeRepository.GetProductTypeByNameAsync(name);

        if (productType == null)
        {
            _logger.LogInformation("Product type not found.");
            return NotFound();
        }

        return Ok(productType);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductTypeById(int id)
    {
        _logger.LogInformation($"Getting product type with id: {id}.");
        var item = await _unitOfWork.ProductTypeRepository.GetByIdAsync(id);

        if (item == null)
        {
            _logger.LogInformation("Product type not found.");
            return NotFound();
        }

        return Ok(item);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProductType(int id, string productName)
    {
        try
        {
            _logger.LogInformation($"Updating product type with id: {id}.");
            var productType = await _unitOfWork.ProductTypeRepository.GetByIdAsync(id);
            if (productType == null)
            {
                _logger.LogInformation("Product type not found.");
                return NotFound("Product type not found");
            }
            productType.TypeName = productName;
            await _unitOfWork.ProductTypeRepository.UpdateAsync(productType);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> PostProductType([FromBody]string productName)
    {
        try
        {
            _logger.LogInformation("Adding new product type.");
            var productType = new ProductType { TypeName = productName };
            await _unitOfWork.ProductTypeRepository.InsertAsync(productType);
            return CreatedAtAction("GetAllProductTypes", new { id = productType.ProductTypeId }, productType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new product type.");
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductType(int id)
    {
        _logger.LogInformation($"Deleting product type with id: {id}.");
        await _unitOfWork.ProductTypeRepository.DeleteAsync(id);
        return NoContent();
    }
}