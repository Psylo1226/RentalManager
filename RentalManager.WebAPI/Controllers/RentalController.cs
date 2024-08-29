using Microsoft.AspNetCore.Mvc;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;

namespace RentalManager.WebAPI.Controllers
{
	public class RentalDto
	{
		public int CustomerId { get; set; }
		public DateTime RentalDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public decimal FullPrice { get; set; }
	}
	[Route("api/[controller]")]
	[ApiController]
	public class RentalController : ControllerBase
	{
		private readonly IRentalManagerUnitOfWork _unitOfWork;
		private readonly ILogger<RentalController> _logger;
		public RentalController(IRentalManagerUnitOfWork unitOfWork, ILogger<RentalController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllRentals()
		{
			_logger.LogInformation("Getting all rentals.");
			var items = await _unitOfWork.RentalRepository.GetAllAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRentalById(int id)
		{
			_logger.LogInformation($"Getting rental with id: {id}.");
			var item = await _unitOfWork.RentalRepository.GetRentalByIdAsync(id);

			if (item == null)
			{
				_logger.LogInformation("Rental not found.");
				return NotFound();
			}

			return Ok(item);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutRental(int id, RentalDto rentalDto)
		{
			try
			{
				_logger.LogInformation($"Updating rental with id: {id}.");
				var rental = await _unitOfWork.RentalRepository.GetRentalByIdAsync(id);
				if (rental == null)
				{
					_logger.LogInformation("Rental not found.");
					return NotFound("Rental not found");
				}
				var customer = await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(rentalDto.CustomerId);
				if (customer == null)
				{
					_logger.LogInformation("Customer not found.");
					return NotFound("Customer not found");
				}
				rental.CustomerId = rentalDto.CustomerId;
				rental.Customer = customer;
				rental.RentalDate = rentalDto.RentalDate;
				rental.ReturnDate = rentalDto.ReturnDate;
				rental.FullPrice = rentalDto.FullPrice;
				
				await _unitOfWork.RentalRepository.UpdateAsync(rental);
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating rental.");
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> PostRental(RentalDto rentalDto)
		{
			try
			{
				var customer = await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(rentalDto.CustomerId);
				if (customer == null)
				{
					_logger.LogInformation("Customer not found.");
					return BadRequest("Customer not found");
				}

				var rental = new Rental
				{
					CustomerId = rentalDto.CustomerId,
					RentalDate = rentalDto.RentalDate,
					ReturnDate = rentalDto.ReturnDate,
					FullPrice = rentalDto.FullPrice,
					Customer = customer
				};
				await _unitOfWork.RentalRepository.InsertAsync(rental);

				return CreatedAtAction("GetRentalById", new { id = rental.RentalId }, rental);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding new rental.");
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRental(int id)
		{
			_logger.LogInformation($"Deleting rental with id: {id}.");
			var rental = await _unitOfWork.RentalRepository.GetRentalByIdAsync(id);
			if (rental != null)
			{
				foreach (var rentalDetail in rental.RentalDetails)
				{
					rentalDetail.Product.QuantityInStock += rentalDetail.Quantity;
				}
			}
			await _unitOfWork.RentalRepository.DeleteAsync(id);

			return NoContent();
		}
	}

}
