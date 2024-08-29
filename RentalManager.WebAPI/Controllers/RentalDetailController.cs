using Microsoft.AspNetCore.Mvc;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;

namespace RentalManager.WebAPI.Controllers
{
	public class RentalDetailDto
	{
		public int RentalId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
	[Route("api/[controller]")]
	[ApiController]
	public class RentalDetailController : ControllerBase
	{
		private readonly IRentalManagerUnitOfWork _unitOfWork;
		private readonly ILogger<RentalDetailController> _logger;

		public RentalDetailController(IRentalManagerUnitOfWork unitOfWork, ILogger<RentalDetailController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllRentalDetails()
		{
			_logger.LogInformation("Getting all rental details.");
			var items =  await _unitOfWork.RentalDetailRepository.GetAllAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRentalDetailsById(int id)
		{
			_logger.LogInformation($"Getting rental detail with id: {id}.");
			var item = await _unitOfWork.RentalDetailRepository.GetRentDetailByIdAsync(id);

			if (item == null)
			{
				_logger.LogInformation("Rental detail not found.");
				return NotFound("Rental detail not found");
			}

			return Ok(item);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> PutRentalDetail(int id, RentalDetailDto rentalDetailDto)
		{
			try
			{
				_logger.LogInformation("Updating rental detail.");
				var rentalDetail = await _unitOfWork.RentalDetailRepository.GetRentDetailByIdAsync(id);
				if (rentalDetail == null)
				{
					_logger.LogInformation("Rental detail not found.");
					return NotFound("Rental detail not found");
				}
				var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(rentalDetailDto.ProductId);
				if (product == null)
				{
					_logger.LogInformation("Product not found.");
					return NotFound("Product not found");
				}
				rentalDetail.ProductId = rentalDetailDto.ProductId;
				rentalDetail.Product = product;
				rentalDetail.Quantity = rentalDetailDto.Quantity;
				await _unitOfWork.RentalDetailRepository.UpdateAsync(rentalDetail);
				await _unitOfWork.RentalRepository.CalculateRentalPriceAsync(rentalDetail.RentalId);
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating rental detail.");
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> PostRentalDetail(RentalDetailDto rentalDetailDto)
		{
			try
			{
				_logger.LogInformation("Adding new rental detail.");
				var rental = await _unitOfWork.RentalRepository.GetRentalByIdAsync(rentalDetailDto.RentalId);
				if (rental != null)
				{
					var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(rentalDetailDto.ProductId);
					if (product != null)
					{
						if(product.QuantityInStock < rentalDetailDto.Quantity)
						{
							_logger.LogInformation("Not enough products in stock.");
							return BadRequest("Not enough products in stock");
						}
						product.QuantityInStock -= rentalDetailDto.Quantity;
						var rentalDetail = new RentalDetail
						{
							RentalId = rentalDetailDto.RentalId,
							Product = product,
							Quantity = rentalDetailDto.Quantity,
							Rental = rental
						};
						await _unitOfWork.RentalDetailRepository.InsertAsync(rentalDetail);
						await _unitOfWork.RentalRepository.CalculateRentalPriceAsync(rentalDetail.RentalId);
						return CreatedAtAction("GetRentalDetailsById", new { id = rentalDetail.RentalDetailId }, rentalDetail);
					}
				}
				_logger.LogInformation("Rental or Product not found.");
				return BadRequest("Rental or Product not found");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding new rental detail.");
				return BadRequest(ex.Message);
			}	
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRentalDetail(int id)
		{
			try
			{
				_logger.LogInformation($"Deleting rental detail with id: {id}.");
				var rental = await _unitOfWork.RentalDetailRepository.GetRentDetailByIdAsync(id);
				if (rental != null)
				{
					var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(rental.ProductId);
					product!.QuantityInStock += rental.Quantity;
					int rentalId = rental.RentalId;
					await _unitOfWork.RentalDetailRepository.DeleteAsync(id);
					await _unitOfWork.RentalRepository.CalculateRentalPriceAsync(rentalId);
				}

				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting rental detail.");
				return BadRequest(ex.Message);
			}
		}
	}

}
