using Kosystem.Repository.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Kosystem
{
    public class Program
    {
        private readonly string[] _args;
        private readonly IHost _host;

        public Program(string[] args)
        {
            _args = args;
            _host = CreateHostBuilder().Build();
        }

        public static void Main(string[] args)
        {
            new Program(args).Run();
        }

        private void Run()
        {
            _host.CreateKosystemEfDbIfNotExists();
            _host.Run();
        }

        private IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder(_args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
