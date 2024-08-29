using RentalManager.Domain.Models;

namespace RentalManager.Domain.Contracts;

public interface IRentalRepository : IRepository<Rental>
{
    Task<IList<Rental>> GetAllAsync();
    Task<Rental?> GetRentalByIdAsync(int id);
    Task InsertAsync(Rental entity);
    Task UpdateAsync(Rental entity);
    Task DeleteAsync(int id);
    Task<decimal> CalculateRentalPriceAsync(int id);
}