using System.Linq.Expressions;

namespace OrderManagementSystem.BL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(object Id);
        T GetByIdAsNoTracking(object Id);
        Task<T> GetByIdAsync(object Id);
        T Insert(T entity);
        T Update(T entity);
        List<T> UpdateRange(List<T> entities);
        Task<T> UpdateAsync(T entity);
        void Delete(object Id);
        int SaveChanged();
        public IQueryable<T> GetWithIncludes(params Expression<Func<T, object>>[] includes);
        List<T> InsertRange(List<T> entities);
    }
}
