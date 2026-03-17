using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Entities.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Persistence
{
    public class InventoryManagerContext : DbContext
    {
        public InventoryManagerContext(DbContextOptions<InventoryManagerContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryManagerContext).Assembly);

        }
    }
}