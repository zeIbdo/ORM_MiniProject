using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Configurations
{
    public class PaymentsConfiguration:IEntityTypeConfiguration<Payments>
    {
       

        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(p => p.PaymentDate).IsRequired();
            builder.HasCheckConstraint("CK_Payments_Amount_Positive", "[Amount]>0");
        }
    }
}
