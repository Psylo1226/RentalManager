using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance.EntityConfiguration;

namespace RentalManager.Infrastructure.Persistance
{
    public sealed class RentalManagerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail?> RentalDetails { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public RentalManagerDbContext(DbContextOptions<RentalManagerDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new RentalConfiguration());
            builder.ApplyConfiguration(new RentalDetailConfiguration());
            builder.ApplyConfiguration(new ProductTypeConfiguration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteConnectionString = "Data Source=RentalManagerDb.db";
            optionsBuilder.UseSqlite(sqliteConnectionString);
        }
    }
}
