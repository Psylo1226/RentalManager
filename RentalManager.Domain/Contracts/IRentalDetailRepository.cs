using RentalManager.Domain.Models;

namespace RentalManager.Domain.Contracts;

public interface IRentalDetailRepository : IRepository<RentalDetail>
{
    Task<IList<RentalDetail?>> GetAllAsync();

    Task<RentalDetail?> GetRentDetailByIdAsync(int id);

    Task InsertAsync(RentalDetail entity);

    Task UpdateAsync(RentalDetail entity);

    Task DeleteAsync(int id);
}