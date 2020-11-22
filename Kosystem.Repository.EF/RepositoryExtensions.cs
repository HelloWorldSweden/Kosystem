using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kosystem.Repository.EF
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddKosystemEfRepository(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return services
                .AddDbContextFactory<KosystemDbContext>(optionsAction)
                .AddSingleton<IPersonRepository, EfPersonRepository>()
                .AddSingleton<IRoomRepository, EfRoomRepository>();
        }

        public static IHost CreateKosystemEfDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var optionsBuilder = new DbContextOptionsBuilder(services.GetRequiredService<DbContextOptions>())
                .UseLoggerFactory(services.GetRequiredService<ILoggerFactory>());

            using var ctx = new KosystemDbContext(optionsBuilder.Options);

            ctx.Database.EnsureCreated();

            return host;
        }
    }
}
