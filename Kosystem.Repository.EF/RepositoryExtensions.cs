using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kosystem.Repository.EF
{
    public static class RepositoryExtensions
    {
        private class KosystemEfRepositoryConfiguration : IKosystemRepositoryConfiguration
        {
            private Func<IServiceProvider, IPersonRepository, IPersonRepository>? _personRepoFactory;
            private Func<IServiceProvider, IRoomRepository, IRoomRepository>? _roomRepoFactory;
            
            public IKosystemRepositoryConfiguration AddPersonRepositoryMiddleware(Func<IServiceProvider, IPersonRepository, IPersonRepository> implementationFactory)
            {
                _personRepoFactory = implementationFactory;
                return this;
            }

            public IKosystemRepositoryConfiguration AddRoomRepositoryMiddleware(Func<IServiceProvider, IRoomRepository, IRoomRepository> implementationFactory)
            {
                _roomRepoFactory = implementationFactory;
                return this;
            }

            public IPersonRepository CreatePersonFactory(IServiceProvider provider)
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<KosystemDbContext>>();
                var logger = provider.GetRequiredService<ILogger<EfPersonRepository>>();
                var repo = new EfPersonRepository(dbContextFactory, logger);

                if (_personRepoFactory is not null)
                {
                    return _personRepoFactory(provider, repo);
                }

                return repo;
            }

            public IRoomRepository CreateRoomFactory(IServiceProvider provider)
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<KosystemDbContext>>();
                var logger = provider.GetRequiredService<ILogger<EfRoomRepository>>();
                var repo = new EfRoomRepository(dbContextFactory, logger);

                if (_roomRepoFactory is not null)
                {
                    return _roomRepoFactory(provider, repo);
                }

                return repo;
            }
        }

        public static IKosystemRepositoryConfiguration AddKosystemEfRepository(
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

        public static IKosystemRepositoryConfiguration AddKosystemEfRepository(
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
