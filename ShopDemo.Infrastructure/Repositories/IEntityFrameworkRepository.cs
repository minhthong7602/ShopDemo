using System.Linq.Expressions;

namespace ShopDemo.Infrastructure.Repositories
{
    public interface IEntityFrameworkRepository<T> where T : class
    {
        Task<T> Insert(T entity, CancellationToken cancellationToken);

        Task Delete(T entity);

        Task<T> Update(T entity, CancellationToken cancellationToken);

        Task<T> GetById(Expression<Func<T, bool>> where, CancellationToken cancellationToken);

        Task<T> GetSingleByCondition(Expression<Func<T, bool>> where, string[] includes = null);

        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, CancellationToken cancellationToken);

        IEnumerable<T> GetMulti(Expression<Func<T, bool>> where, string[] includes);

        Task<IEnumerable<T>> GetAll();

        IEnumerable<T> GetAll(string[] includes);

        Task<int> Count();

        Task InsertList(List<T> entities, CancellationToken cancellationToken);

        Task DeleteList(List<T> entities);

        Task DeleteWhere(Expression<Func<T, bool>> where, CancellationToken cancellationToken);

        Task<bool> Any(Expression<Func<T, bool>> where, CancellationToken cancellationToken);
    }
}
