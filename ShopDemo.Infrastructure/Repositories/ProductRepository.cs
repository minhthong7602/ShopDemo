using ShopDemo.Domain.Entities;

namespace ShopDemo.Infrastructure.Repositories
{
    public interface IProductRepository : IEntityFrameworkRepository<Product>
    {
    }

    public class ProductRepository : EntityFrameworkRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
