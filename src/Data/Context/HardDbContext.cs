using Hard.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hard.Data.Context
{
    public class HardDbContext : DbContext
    {        
        public HardDbContext(DbContextOptions<HardDbContext> options) : base(options)
        {            
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HardDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
