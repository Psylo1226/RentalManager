using System.Linq.Expressions;

namespace RentalManager.Domain.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    int Count();
    TEntity? GetValueOrDefault(Guid id);
    IList<TEntity> GetAll();
    IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    void Insert(TEntity entity);
    void Delete(TEntity entity);
}