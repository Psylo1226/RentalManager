namespace RentalManager.Domain.Contracts;

public interface IRentalManagerUnitOfWork : IDisposable
{
    ICustomerRepository CustomerRepository { get; }
    IProductRepository ProductRepository { get; }
    IRentalRepository RentalRepository { get; }
    IRentalDetailRepository RentalDetailRepository { get; }
    IProductTypeRepository ProductTypeRepository { get; }
    Task<int> SaveChangesAsync();
    void Commit(); 
}