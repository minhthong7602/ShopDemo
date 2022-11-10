using Microsoft.EntityFrameworkCore;

namespace ShopDemo.Web.Extensions
{
    public static class HostExtensions
    {
        public static void AddAppConfigurations(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });
        }

        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var dbContext = services.GetService<TContext>();

                if (dbContext == null) return host;
                try
                {
                    logger.LogInformation("Migrating mysql database.");
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                    logger.LogInformation("Migrated mysql database.");
                    seeder(dbContext, services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the mysql database");
                }
            }

            return host;
        }
    }
}
