using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Data
{
    public class InventoryManagerContext : DbContext
    {
        public InventoryManagerContext(DbContextOptions<InventoryManagerContext> options) : base(options)
        {
        }

        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.InventoryMovement> InventoryMovements { get; set; }
    }
}