using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_MiniProject.Models;

namespace ORM_MiniProject.Configurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(p => p.Stock).IsRequired();
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.CreatedTime).IsRequired();
            builder.Property(p => p.UpdatedTime).IsRequired();
            builder.HasCheckConstraint("CK_Products_Price_Positive", "[Price] > 0");
            builder.HasCheckConstraint("CK_Products_Stock_NonNegative", "[Stock] >= 0");
        }
    }
}
