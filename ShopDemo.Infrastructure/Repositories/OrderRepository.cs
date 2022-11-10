using ShopDemo.Domain.Entities;
using ShopDemo.Infrastructure.DTO;

namespace ShopDemo.Infrastructure.Repositories
{
    public interface IOrderRepository : IEntityFrameworkRepository<Order>
    {
        IQueryable<OrderViewDTO> GetAllOrderView(string keyword, string sortOrder);
    }

    public class OrderRepository : EntityFrameworkRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<OrderViewDTO> GetAllOrderView(string keyword, string sortOrder)
        {
            var orders = from o in DbContext.Orders
                         join p in DbContext.Products on o.ProductId equals p.Id
                         join c in DbContext.Categories on p.CategoryId equals c.Id
                         join cus in DbContext.Customers on o.CustomerId equals cus.Id
                         where (string.IsNullOrEmpty(keyword) || c.Name.Contains(keyword))
                         select new OrderViewDTO
                         {
                             Id = o.Id,
                             CategoryName = c.Name,
                             ProductName = p.Name,
                             CustomerName = cus.Name,
                             OrderDate = o.OrderDate,
                             Amount = o.Amount
                         };

            if(string.IsNullOrEmpty(sortOrder)) return orders;

            if (sortOrder.Contains("product"))
            {
                orders = sortOrder == "product_desc" ? orders.OrderByDescending(o => o.ProductName) : orders.OrderBy(o => o.ProductName);
            }
            else if (sortOrder.Contains("category"))
            {
                orders = sortOrder == "category_desc" ? orders.OrderByDescending(o => o.CategoryName) : orders.OrderBy(o => o.CategoryName);
            }
            else if (sortOrder.Contains("customer"))
            {
                orders = sortOrder == "customer_desc" ? orders.OrderByDescending(o => o.CustomerName) : orders.OrderBy(o => o.CustomerName);
            }
            else
            {
                orders = orders.OrderBy(o => o.ProductName);
            }
            return orders;
        }
    }
}
