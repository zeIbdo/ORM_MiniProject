using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_MiniProject.Models;

namespace ORM_MiniProject.Configurations
{
    public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.OrderDate).IsRequired();
            builder.Property(x => x.TotalAmount).IsRequired().HasColumnType("decimal(10,2)");
            builder.HasCheckConstraint("CK_Orders_TotalAmount_Positive", "[TotalAmount] >= 0");

        }
    }
}
