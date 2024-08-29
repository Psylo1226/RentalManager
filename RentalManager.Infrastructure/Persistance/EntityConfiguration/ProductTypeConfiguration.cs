using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalManager.Domain.Models;

namespace RentalManager.Infrastructure.Persistance.EntityConfiguration;

public class ProductTypeConfiguration: IEntityTypeConfiguration<ProductType>
{

        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(p => p.ProductTypeId);

            builder.Property(p => p.ProductTypeId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.TypeName)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(p=>p.TypeName).IsUnique();
        }
    
}