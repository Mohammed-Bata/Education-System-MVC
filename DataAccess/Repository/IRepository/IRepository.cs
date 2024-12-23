using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,string ? includeProperties=null,int pageSize = 0,int pageNumber = 0);
        Task<T> GetAsync(Expression<Func<T, bool>> filter,string? includeProperties = null);
        Task AddAsync(T entity);
        void Remove(T entity);
        Task SaveAsync();

    }
}
