using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;

namespace RentalManager.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly RentalManagerDbContext _rentalManagerDbContext;

    public CustomerRepository(RentalManagerDbContext context) : base(context)
    {
        _rentalManagerDbContext = context;
    }

    public async Task<IList<Customer>> GetAllAsync()
    {
        return await _rentalManagerDbContext.Customers.ToListAsync();
    }
    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _rentalManagerDbContext.Customers.FirstOrDefaultAsync(c=>c.CustomerId == id);
    }
    public async Task InsertAsync(Customer entity)
    {
        await  _rentalManagerDbContext.Customers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer entity)
    {
        _rentalManagerDbContext.Customers.Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var customer = await GetCustomerByIdAsync(id);
        if (customer != null)
        {
            _rentalManagerDbContext.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}