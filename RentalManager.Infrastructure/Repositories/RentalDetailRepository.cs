using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;

namespace RentalManager.Infrastructure.Repositories
{
	// Implementacja repozytoriów specyficznych
	public class RentalDetailRepository : Repository<RentalDetail>, IRentalDetailRepository
	{
		private readonly RentalManagerDbContext _rentalManagerDbContext;

		public RentalDetailRepository(RentalManagerDbContext context)
			: base(context)
		{
			_rentalManagerDbContext = context;
		}
		public async Task<IList<RentalDetail?>> GetAllAsync()
		{
			return await _rentalManagerDbContext.RentalDetails.ToListAsync();
		}
		public async Task<RentalDetail?> GetRentDetailByIdAsync(int id)
		{
			return await _rentalManagerDbContext.RentalDetails.FirstOrDefaultAsync(x => x.RentalDetailId == id);
		}
		public async Task InsertAsync(RentalDetail entity)
		{
			await _rentalManagerDbContext.RentalDetails.AddAsync(entity);
			await _rentalManagerDbContext.SaveChangesAsync();
		}
		public async Task UpdateAsync(RentalDetail entity)
		{
			_rentalManagerDbContext.RentalDetails.Update(entity);
			await _rentalManagerDbContext.SaveChangesAsync();
		}
		public async Task DeleteAsync(int id)
		{
			var rentalDetail = await _rentalManagerDbContext.RentalDetails.FindAsync(id);
			if (rentalDetail != null)
			{
				_rentalManagerDbContext.RentalDetails.Remove(rentalDetail);
				await _rentalManagerDbContext.SaveChangesAsync();
			}
		}
	}
}