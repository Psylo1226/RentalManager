using RentalManager.Domain.Contracts;
using RentalManager.Infrastructure.Repositories;

namespace RentalManager.Infrastructure.Persistance
{
    public class RentalManagerUnitOfWork : IRentalManagerUnitOfWork
    {
        private readonly RentalManagerDbContext _context;

        public RentalManagerUnitOfWork(RentalManagerDbContext context)
        {
            _context = context;
            CustomerRepository = new CustomerRepository(context);
            ProductRepository = new ProductRepository(context);
            RentalRepository = new RentalRepository(context);
            RentalDetailRepository = new RentalDetailRepository(context);
            ProductTypeRepository = new ProductTypeRepository(context);
        }
        
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public ICustomerRepository CustomerRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IRentalRepository RentalRepository { get; }
        public IRentalDetailRepository RentalDetailRepository { get; }
        public IProductTypeRepository ProductTypeRepository { get; }

    }
}
