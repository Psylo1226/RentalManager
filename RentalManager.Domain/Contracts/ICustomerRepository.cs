using RentalManager.Domain.Models;

namespace RentalManager.Domain.Contracts;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IList<Customer>> GetAllAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task InsertAsync(Customer entity);
    Task UpdateAsync(Customer entity);
    Task DeleteAsync(int id);
    
}