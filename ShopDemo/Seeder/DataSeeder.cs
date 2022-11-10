using ShopDemo.Infrastructure;
using ShopDemo.Domain.Entities;

namespace ShopDemo.Web.Seeder
{
    public class DataSeeder
    {
        public static async Task InitData(ApplicationDbContext dbContext, ILogger logger)
        {
            if(!dbContext.Customers.Any())
            {
                var customer1 = new Customer("Truong Van A", "Ha Nam");
                var customer2 = new Customer("Nguyen Van B", "Ha Noi");

                await dbContext.Customers.AddRangeAsync(new List<Customer> { customer1, customer2 });
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Seeded customers data for ShopDemo DB associated with context {DbContextName}",
                    nameof(ApplicationDbContext));
            }

            if(!dbContext.Categories.Any() && !dbContext.Products.Any())
            {
                var category1 = new Category("Category 1", "Category 1");
                var category2 = new Category("Category 2", "Category 2");
                var category3 = new Category("Category 3", "Category 3");
                await dbContext.Categories.AddRangeAsync(new List<Category> { category1, category2, category3 });
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Seeded categories data for ShopDemo DB associated with context {DbContextName}",
                    nameof(ApplicationDbContext));

                var product1 = new Product("Product 1", category1.Id, 100, "Product 1", 10);
                var product2 = new Product("Product 2", category2.Id, 150, "Product 2", 15);
                var product3 = new Product("Product 3", category3.Id, 200, "Product 3", 20);
                await dbContext.Products.AddRangeAsync(new List<Product> { product1, product2, product3 });
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Seeded products data for ShopDemo DB associated with context {DbContextName}",
                    nameof(ApplicationDbContext));
            }
        }
    }
}
