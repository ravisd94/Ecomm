using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Use HasPrecision instead of HasColumnType for decimal properties
            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.Name).IsRequired();
        }
    }

}
