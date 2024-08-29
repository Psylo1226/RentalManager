using RentalManager.Domain.Models;

namespace RentalManager.Domain.Contracts;

public interface IProductTypeRepository : IRepository<ProductType>
{
    Task<IList<ProductType>> GetAllAsync();
    Task<ProductType?> GetByIdAsync(int id);
    Task<ProductType?> GetProductTypeByNameAsync(string typeName);
    Task InsertAsync(ProductType entity);
    Task UpdateAsync(ProductType entity);
    Task DeleteAsync(int id);
}