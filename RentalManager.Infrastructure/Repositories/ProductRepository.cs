using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;

namespace RentalManager.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly RentalManagerDbContext _rentalManagerDbContext;

        public ProductRepository(RentalManagerDbContext context)
            : base(context)
        {
            _rentalManagerDbContext = context;
        }

        public async Task<IList<Product>> GetAllAsync()
        {
            return await _rentalManagerDbContext.Products.ToListAsync();
        }
        public async Task<IList<Product>> GetAllAsyncWithProductTypes()
        {
            return await _rentalManagerDbContext.Products.Include(p=>p.ProductType).ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _rentalManagerDbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        
        public async Task<Product?> GetProductByIdAsyncWithProductTypes(int id)
        {
            return await _rentalManagerDbContext.Products.Include(p=>p.ProductType)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        
        public async Task InsertAsync(Product entity)
        {
            await _rentalManagerDbContext.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _rentalManagerDbContext.Products.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                _rentalManagerDbContext.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}