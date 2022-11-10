using ShopDemo.Domain.Entities;

namespace ShopDemo.Infrastructure.Repositories
{
    public interface ICustomerRepository : IEntityFrameworkRepository<Customer>
    {
    }

    public class CustomerRepository : EntityFrameworkRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
