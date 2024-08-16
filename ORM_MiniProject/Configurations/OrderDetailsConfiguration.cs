using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_MiniProject.Models;

namespace ORM_MiniProject.Configurations
{
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x=>x.PricePerItem).IsRequired().HasColumnType("decimal(10,2)");
            builder.HasCheckConstraint("CK_OrderDetails_Quantity", "Quantity > 0");
            builder.HasCheckConstraint("CK_OrderDetails_PricePerItem", "PricePerItem > 0");

        }
    }
}
