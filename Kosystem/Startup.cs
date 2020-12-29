using System.Globalization;
using Kosystem.Configuration;
using Kosystem.Events;
using Kosystem.Repository.EF;
using Kosystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kosystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure((System.Action<RequestLocalizationOptions>)(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");

                var cultureInfos = new[] { new CultureInfo("en"), new CultureInfo("sv") };
                options.SupportedCultures = cultureInfos;
                options.SupportedUICultures = cultureInfos;
            }));

            services.AddRazorPages();
            services.AddServerSideBlazor(options => options.DetailedErrors = true);
            services.AddControllers();

            var events = new KosystemEvents();
            services.AddSingleton<IKosystemEventListener>(events);
            services.AddSingleton<IKosystemEventSender>(events);

            services.AddScoped<AuthenticationStateProvider, MyAuthenticationStateProvider>();
            services.AddScoped(sp => {
                var provider = (IAuthSetter)sp.GetRequiredService<AuthenticationStateProvider>();
                return provider;
            });

            services.AddScoped<IPersonSession, PersonSession>();

            services.AddOptions<LoginOptions>()
                .Bind(Configuration.GetSection("Login"))
                .ValidateDataAnnotations();

            services.AddKosystemEfRepository(opt =>
            {
                opt.UseSqlite($"DataSource={nameof(Kosystem)}.db", sqliteOpt =>
                {
                    sqliteOpt.MigrationsAssembly(nameof(Kosystem));
                });
            });

            services.AddSingleton<EventAwareRepositories>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
