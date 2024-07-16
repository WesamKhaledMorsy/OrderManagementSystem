using OrderManagementSystem.DL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderManagementSystem.BL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private DbSet<T> _entity;

        public GenericRepository(AppDBContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public void Delete(object Id)
        {
            T exsisting = GetById(Id);
            _entity.Remove(exsisting);
        }

        public IQueryable<T> GetAll() =>
            _entity;

        public T GetById(object Id) =>
            _entity.Find(Id);

        public async Task<T> GetByIdAsync(object Id) =>
            _entity.Find(Id);

        public T GetByIdAsNoTracking(object Id)
        {
            var entity = _entity.Find(Id);
            if (entity is not null)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;

        }

        public T Insert(T entity)
        {
            _entity.AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _entity.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public List<T> UpdateRange(List<T> entities)
        {
            _context.UpdateRange(entities);
            return entities;
        }
        public List<T> InsertRange(List<T> entities)
        {
            _context.AddRange(entities);
            return entities;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _entity.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;

        }

        public int SaveChanged() =>
           _context.SaveChanges();
        public IQueryable<T> GetWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

    }
}
