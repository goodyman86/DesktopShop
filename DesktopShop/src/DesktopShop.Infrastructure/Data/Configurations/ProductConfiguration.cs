using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DesktopShop.Domain.Entities;

namespace DesktopShop.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.CPU).HasMaxLength(100);
        builder.Property(p => p.RAM).HasMaxLength(50);
        builder.Property(p => p.GPU).HasMaxLength(100);
        builder.Property(p => p.Storage).HasMaxLength(100);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.Property(p => p.ImageUrl).HasMaxLength(500);
        builder.Property(p => p.Description).HasMaxLength(2000);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
