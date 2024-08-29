using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalManager.Domain.Models;

namespace RentalManager.Infrastructure.Persistance.EntityConfiguration
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.HasKey(p => p.RentalId);

            builder.Property(p => p.RentalId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.RentalDate)
                .IsRequired();
            
            builder.Property(p => p.FullPrice).IsRequired();

            builder.HasOne(w => w.Customer)
                .WithMany(k => k.Rentals)
                .HasForeignKey(w => w.CustomerId);

            builder.HasMany(w => w.RentalDetails)
                .WithOne(s => s.Rental)
                .HasForeignKey(s => s.RentalDetailId);
        }
    }
}