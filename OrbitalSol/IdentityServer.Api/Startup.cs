//using Cache.Business;
using IdentityServer.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using IdentityServer.Api.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Newtonsoft;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer.Api.Models;

namespace IdentityServer.Api
{
    public class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }
        const string connectionString = @"DESKTOP-2Q2AIBL;database=IdentityServer;trusted_connection=yes;user id=sa;password=123";
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration.GetSection("Microservices").GetSection("Authority").Value;
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = authority;
                options.EmitStaticAudienceClaim = true;
            })
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(Configuration.GetConnectionString("ConfigurationStore"),
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(Configuration.GetConnectionString("OperationalStore"),
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = false;
                    // options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });
            //services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages();
            //.RegisterMvc();
           // services.AddApplicationInsightsTelemetry();
            // services
            // .AddMvc(options => options.EnableEndpointRouting = false)
            // .AddNewtonsoftJson(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //services.AddScoped<ICopyAsset, CopyAsset>();
            //services.AddScoped<IUrlParse, UrlParse>();
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env
            //ILoggerFactory loggerFactory
            )
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // app.UseDatabaseErrorPage();
                //                SeedData.EnsureSeedData(app.ApplicationServices);
                SeedData.InitializeConfigurationDatabase(app.ApplicationServices, Configuration);
            }

            // loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Warning);
            app.UseStaticFiles();
            app.UseIdentityServer();

            app.UseRequestLocalization();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
            //  app.UseMvcWithDefaultRoute();

        }
      
    }
}