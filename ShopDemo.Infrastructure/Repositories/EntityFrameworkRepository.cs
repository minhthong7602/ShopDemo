using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace ShopDemo.Infrastructure.Repositories
{
    public class EntityFrameworkRepository<T> : IEntityFrameworkRepository<T> where T : class
    {
        #region Field

        private ApplicationDbContext _dbContext;
        private DbSet<T> _dbSet;

        #endregion Field

        #region Ctr

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected ApplicationDbContext DbContext
        {
            get => _dbContext ?? (_dbContext = DbFactory.Init());
        }

        protected DbSet<T> DbSet
        {
            get => _dbSet ?? (_dbSet = DbContext.Set<T>());
        }

        public EntityFrameworkRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbContext = DbFactory.Init();
            _dbSet = DbContext.Set<T>();
        }

        public virtual async Task<int> Count() => await _dbSet.CountAsync();

        public virtual async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAll() => await DbContext.Set<T>().ToListAsync();

        public virtual IEnumerable<T> GetAll(string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = DbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return DbSet.AsQueryable();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> where, CancellationToken cancellationToken) => await DbSet.Where(where).FirstOrDefaultAsync(cancellationToken);
        public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> where, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = DbSet.Where(where).Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include).AsQueryable();
                return await query.FirstOrDefaultAsync();
            }
            return await DbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where, CancellationToken cancellationToken) => await DbSet.Where(where).ToListAsync(cancellationToken);

        public IEnumerable<T> GetMulti(Expression<Func<T, bool>> where, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = DbSet.Where(where).Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include).AsQueryable();
                return query;
            }
            return DbSet.Where(where);
        }

        public async Task<T> Insert(T entity, CancellationToken cancellationToken)
        {
            DbSet.Add(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task InsertList(List<T> entities, CancellationToken cancellationToken)
        {
            entities.ForEach(entity => DbSet.Add(entity));
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteList(List<T> entities)
        {
            entities.ForEach(entity => DbSet.Remove(entity));
            await DbContext.SaveChangesAsync();
        }

        public async Task<T> Update(T entity, CancellationToken cancellationToken)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task DeleteWhere(Expression<Func<T, bool>> where, CancellationToken cancellationToken)
        {
            var entities = DbSet.Where(where).ToList();
            entities.ForEach(entity => _dbSet.Remove(entity));
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> where, CancellationToken cancellationToken)
        {
            return await DbSet.Where(where).AnyAsync(cancellationToken);
        }
        #endregion Ctr
    }
}
