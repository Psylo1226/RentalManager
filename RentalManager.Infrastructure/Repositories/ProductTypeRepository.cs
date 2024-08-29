using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;

namespace RentalManager.Infrastructure.Repositories;

public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
{
    private readonly RentalManagerDbContext _rentalManagerDbContext;
    public ProductTypeRepository(RentalManagerDbContext context) : base(context)
    {
        _rentalManagerDbContext = context;
    }
    public async Task<IList<ProductType>> GetAllAsync()
    {
        return await _rentalManagerDbContext.ProductTypes.ToListAsync();
    }

    public async Task<ProductType?> GetByIdAsync(int id)
    {
        return await _rentalManagerDbContext.ProductTypes.FirstOrDefaultAsync(x => x.ProductTypeId == id);
    }

    public async Task<ProductType?> GetProductTypeByNameAsync(string typeName)
    {
        return await _rentalManagerDbContext.ProductTypes.FirstOrDefaultAsync(x => x.TypeName == typeName);
    }
    public async Task InsertAsync(ProductType entity)
    {
        await _rentalManagerDbContext.ProductTypes.AddAsync(entity);
        await _rentalManagerDbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(ProductType entity)
    {
        _rentalManagerDbContext.ProductTypes.Update(entity);
        await _rentalManagerDbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var productType = await _rentalManagerDbContext.ProductTypes.FindAsync(id);
        if (productType != null)
        {
            _rentalManagerDbContext.ProductTypes.Remove(productType);
            await _rentalManagerDbContext.SaveChangesAsync();
        }
    }
}