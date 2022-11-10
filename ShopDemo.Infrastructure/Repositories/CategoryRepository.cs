using ShopDemo.Domain.Entities;

namespace ShopDemo.Infrastructure.Repositories
{
    public interface ICategoryRepository : IEntityFrameworkRepository<Category>
    {
    }

    public class CategoryRepository : EntityFrameworkRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
