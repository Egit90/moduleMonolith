using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations;

public sealed class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Category).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.ImageFile).HasMaxLength(100);
        builder.Property(x => x.Price).IsRequired();
    }
}