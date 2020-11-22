using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kosystem.Repository.EF
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddKosystemEfRepository(
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        {
            return services
                .AddDbContextFactory<KosystemDbContext>(optionsAction)
                .AddSingleton<IPersonRepository, EfPersonRepository>()
                .AddSingleton<IRoomRepository, EfRoomRepository>();
        }

        public static IServiceCollection AddKosystemEfRepository(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return AddKosystemEfRepository(services, (_, opt) => optionsAction(opt));
        }

        public static IHost ApplyKosystemMigration(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var ctxFactory = services.GetRequiredService<IDbContextFactory<KosystemDbContext>>();
            using var ctx = ctxFactory.CreateDbContext();

            ctx.Database.Migrate();

            return host;
        }
    }
}
