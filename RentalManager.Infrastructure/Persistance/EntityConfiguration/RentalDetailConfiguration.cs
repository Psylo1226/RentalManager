using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalManager.Domain.Models;

namespace RentalManager.Infrastructure.Persistance.EntityConfiguration
{
    public class RentalDetailConfiguration : IEntityTypeConfiguration<RentalDetail>
    {
        public void Configure(EntityTypeBuilder<RentalDetail> builder)
        {
            builder.HasKey(p => p.RentalDetailId);
            builder.Property(p => p.RentalDetailId)
                .ValueGeneratedOnAdd(); 
            
            builder.HasOne(s => s.Rental)
                .WithMany(w => w.RentalDetails)
                .HasForeignKey(s => s.RentalId);

            builder.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);
        }
    }
}
