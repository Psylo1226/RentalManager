using Microsoft.AspNetCore.Mvc;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;

namespace RentalManager.WebAPI.Controllers
{
    namespace RentalManager.Controllers
    {
        public class CustomerDto
        {
            public string FirstName { get; set; } = null!;
            public string Surname { get; set; } = null!;
            public string Address { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string PhoneNumber { get; set; } = null!;
        }

        [Route("api/[controller]")]
        [ApiController]
        public class CustomerController : ControllerBase
        {
            private readonly IRentalManagerUnitOfWork _unitOfWork;
            private readonly ILogger<CustomerController> _logger;

            public CustomerController(IRentalManagerUnitOfWork unitOfWork, ILogger<CustomerController> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            [HttpGet]
            public async Task<IActionResult> GetClients()
            {
                _logger.LogInformation("Getting all customers.");
                var items = await _unitOfWork.CustomerRepository.GetAllAsync();
                return Ok(items);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetClientById(int id)
            {
                _logger.LogInformation($"Getting customer with id: {id}.");
                var item = await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(id);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutCustomer(int id, CustomerDto customerDto)
            {
                try
                {
                    _logger.LogInformation($"Updating customer with id: {id}.");
                    var customer = await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(id);
                        if(customer == null)
                    {
                        _logger.LogInformation($"Customer with id: {id} not found.");
                        return NotFound();
                    }

                    customer.FirstName = customerDto.FirstName;
                    customer.Surname = customerDto.Surname;
                    customer.Address = customerDto.Address;
                    customer.Email = customerDto.Email;
                    customer.PhoneNumber = customerDto.PhoneNumber;
                    await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating customer.");
                    return BadRequest(ex.Message);
                }
            }

            [HttpPost]
            public async Task<IActionResult> PostCustomer(CustomerDto customerDto)
            {
                try
                {
                    _logger.LogInformation("Adding new customer.");
                    var customer = new Customer
                    {
                        FirstName = customerDto.FirstName,
                        Surname = customerDto.Surname,
                        Address = customerDto.Address,
                        Email = customerDto.Email,
                        PhoneNumber = customerDto.PhoneNumber
                    };

                    await _unitOfWork.CustomerRepository.InsertAsync(customer);

                    return CreatedAtAction("GetClientById", new { id = customer.CustomerId }, customer);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding new customer.");
                    return BadRequest(ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteClient(int id)
            {
                _logger.LogInformation($"Deleting customer with id: {id}.");
                await _unitOfWork.CustomerRepository.DeleteAsync(id);
                return NoContent();
            }
        }
    }
}