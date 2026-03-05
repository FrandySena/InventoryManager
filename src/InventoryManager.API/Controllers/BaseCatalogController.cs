using InventoryManager.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseCatalogController<T> : ControllerBase where T : class
    {
        protected readonly InventoryManagerContext _context;
        protected readonly DbSet<T> Entity;

        public BaseCatalogController(InventoryManagerContext context)
        {
            _context = context;
            Entity = _context.Set<T>();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = Entity.ToList();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = Entity.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Create(T entity)
        {
            Entity.Add(entity);
            _context.SaveChanges();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, T entity)
        {
            if (entity == null)
            {
                return BadRequest("Entity cannot be null");
            }
            var esistingEntity = Entity.Find(id);
            if (esistingEntity == null)
            {
                return NotFound();

            }

            _context.Entry(esistingEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = Entity.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            Entity.Remove(entity);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
