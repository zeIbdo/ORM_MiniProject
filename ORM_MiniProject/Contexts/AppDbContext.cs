using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORM_MiniProject.Utils;
using ORM_MiniProject.Models;
using ORM_MiniProject.Configurations;

namespace ORM_MiniProject.Contexts
{
    public class AppDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Contants.connectionString);
            base.OnConfiguring(optionsBuilder);
        }
        DbSet<Orders> Orders { get; set; } = null!;
        DbSet<Payments> Payment { get; set; }= null!;
        DbSet<Products> Products { get; set; }=null!;
        DbSet<Users> Users { get; set; } = null!;
        DbSet<OrderDetails> OrderDetails { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
