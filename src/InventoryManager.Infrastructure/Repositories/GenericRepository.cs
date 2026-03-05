using InventoryManager.Persistence;

namespace InventoryManager.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : class
    {
        private readonly InventoryManagerContext _context;

        public GenericRepository(InventoryManagerContext context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
