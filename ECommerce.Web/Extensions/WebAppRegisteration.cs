using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extensions
{
    public static class WebAppRegisteration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbContextService = Scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();

            if (PendingMigrations.Any())
                await dbContextService.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();

            var DataInitilizerService = Scope.ServiceProvider.GetRequiredService<IDatainitilizer>();
            await DataInitilizerService.InitializeAsync();

            return app;
        }
    }
}
