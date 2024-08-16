using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ORM_MiniProject.Models;

namespace ORM_MiniProject.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.FullName).IsRequired().HasMaxLength(100);
            builder.Property(a=>a.Address).IsRequired().HasMaxLength(255);
            builder.Property(a=>a.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(a => a.Email).IsUnique();
        }
    }
}
