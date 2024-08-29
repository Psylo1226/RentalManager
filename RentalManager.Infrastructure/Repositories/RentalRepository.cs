using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;

namespace RentalManager.Infrastructure.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        private readonly RentalManagerDbContext _rentalManagerDbContext;

        public RentalRepository(RentalManagerDbContext context)
            : base(context)
        {
            _rentalManagerDbContext = context;
        }
        public async Task<IList<Rental>> GetAllAsync()
        {
            return await _rentalManagerDbContext.Rentals.Include(r => r.RentalDetails).ToListAsync();
        }
        public async Task<Rental?> GetRentalByIdAsync(int id)
        {
            return await _rentalManagerDbContext.Rentals.Include(r => r.RentalDetails)
                .FirstOrDefaultAsync(r => r.RentalId == id);
        }
        public async Task InsertAsync(Rental entity)
        {
            await _rentalManagerDbContext.Rentals.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Rental entity)
        {
            _rentalManagerDbContext.Rentals.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var rental = await GetRentalByIdAsync(id);
            if (rental != null)
            {
                _rentalManagerDbContext.Rentals.Remove(rental);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> CalculateRentalPriceAsync(int id)
        {
            var rental = await _rentalManagerDbContext.Rentals.Include(r=>r.RentalDetails).ThenInclude(rd=>rd.Product).FirstOrDefaultAsync(r => r.RentalId == id);
            if (rental != null)
            {
                decimal totalPrice = 0;
                foreach (var rentalDetail in rental.RentalDetails)
                {
                    totalPrice += rentalDetail.Product.Price * rentalDetail.Quantity;
                }
                rental.FullPrice = totalPrice;
                await _context.SaveChangesAsync();
                return totalPrice;
            }
            throw new Exception("Rental not found");
        }
    }
}