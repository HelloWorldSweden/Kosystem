using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kosystem.Repository.EF
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddKosystemEfRepository(
            this IServiceCollection services)
        {
            return services
                .AddDbContextFactory<KosystemDbContext>()
                .AddSingleton<IPersonRepository, EfPersonRepository>()
                .AddSingleton<IRoomRepository, EfRoomRepository>();
        }

        public static IHost CreateKosystemEfDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var ctx = new KosystemDbContext();

            ctx.Database.Migrate();

            return host;
        }
    }
}
