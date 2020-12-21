using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kosystem.Repository.EF
{
    public static class RepositoryExtensions
    {
        private class KosystemEfRepositoryConfiguration : KosystemRepositoryConfiguration
        {
            protected override IPersonRepository CreateInitialPersonRepository(IServiceProvider provider)
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<KosystemDbContext>>();
                var logger = provider.GetRequiredService<ILogger<EfPersonRepository>>();
                return new EfPersonRepository(dbContextFactory, logger);
            }

            protected override IRoomRepository CreateInitialRoomRepository(IServiceProvider provider)
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<KosystemDbContext>>();
                var logger = provider.GetRequiredService<ILogger<EfRoomRepository>>();
                return new EfRoomRepository(dbContextFactory, logger);
            }
        }

        public static KosystemRepositoryConfiguration AddKosystemEfRepository(
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        {
            var conf = new KosystemEfRepositoryConfiguration();

            services
                .AddDbContextFactory<KosystemDbContext>(optionsAction)
                .AddSingleton(conf.CreatePersonFactory)
                .AddSingleton(conf.CreateRoomFactory);

            return conf;
        }

        public static KosystemRepositoryConfiguration AddKosystemEfRepository(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return AddKosystemEfRepository(services, (_, opt) => optionsAction(opt));
        }

        public static IHost PrepareKosystemDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var ctxFactory = services.GetRequiredService<IDbContextFactory<KosystemDbContext>>();
            using var ctx = ctxFactory.CreateDbContext();

            ctx.Database.Migrate();
            ctx.People.RemoveRange(ctx.People);
            ctx.SaveChanges();

            return host;
        }
    }
}
