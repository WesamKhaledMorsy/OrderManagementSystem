using OrderManagementSystem.BL.Repository;

namespace OrderManagementSystem.BL.UnitOfWork
{
    public interface IUnitOfWork<T> where T : class
    {
        IGenericRepository<T> Repository { get; }
        void Save();
    }
}
