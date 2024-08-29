using RentalManager.Domain.Models;

namespace RentalManager.Domain.Contracts;

public interface IProductRepository : IRepository<Product>
{
    Task<IList<Product>> GetAllAsync();
    Task<IList<Product>> GetAllAsyncWithProductTypes();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductByIdAsyncWithProductTypes(int id);
    Task InsertAsync(Product entity);
    Task UpdateAsync(Product entity);
    Task DeleteAsync(int id);
}