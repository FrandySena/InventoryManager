using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Entities.Catalogs;
using InventoryManager.Persistence;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace InventoryManager.Infrastructure.Repositories
{
    public class UnitOfWork
    {
        private readonly InventoryManagerContext _context;
        private GenericRepository<Category> _categoryRepository;
        private ProductRepository _productRepository;
        private GenericRepository<InventoryMovement> _inventoryMovementRepository;

        public UnitOfWork(InventoryManagerContext context, 
            GenericRepository<Category> categoryRepository, 
            ProductRepository productRepository,
            GenericRepository<InventoryMovement> inventoryMovementRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _inventoryMovementRepository = inventoryMovementRepository;
        }

        public GenericRepository<Category> CategoryRepository => _categoryRepository;
        public ProductRepository ProductRepository => _productRepository;
        public GenericRepository<InventoryMovement> InventoryMovement => _inventoryMovementRepository;

        public void Complete()
        {
            _context.SaveChanges();
        }
        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

    }
}
