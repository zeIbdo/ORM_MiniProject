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
            builder.HasOne(x=>x.User).WithMany(o=>o.Orders).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.TotalAmount).IsRequired().HasColumnType("decimal(10,2)");
            builder.HasMany(x=>x.OrderDetails).WithOne(u=>u.Order).HasForeignKey(u=>u.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasCheckConstraint("CK_Orders_TotalAmount_Positive", "[TotalAmount] > 0");

        }
    }
}
