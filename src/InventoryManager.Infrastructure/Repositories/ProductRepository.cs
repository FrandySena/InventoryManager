using InventoryManager.Domain.Entities;
using InventoryManager.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {

        private readonly InventoryManagerContext _context;

        public ProductRepository(InventoryManagerContext context) : base(context)
        {
            _context = context;
        }

        public List<Product> GetProductsWithCategory()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }
    }
}