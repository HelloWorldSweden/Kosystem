using Kosystem.Repository.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Kosystem
{
    public class Program
    {
        private readonly IHost _host;

        public Program(string[] args)
        {
            _host = CreateHostBuilder(args).Build();
        }

        public static void Main(string[] args)
        {
            new Program(args).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private void Run()
        {
            _host.PrepareKosystemDatabase();
            _host.Run();
        }
    }
}
