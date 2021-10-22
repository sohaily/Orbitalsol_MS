using IdentityServer.API.Data;
using IdentityServer.API.Models;
using IdentityServer.API.Services;
using IdentityServer.API.Services.Profile;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.API.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UserStore")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Tokens.ChangePhoneNumberTokenProvider = "Phone";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var identitySettings = new IdentityAppSettings();
            configuration.GetSection("IdentityAppSettings").Bind(identitySettings);
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(identitySettings.TokenLifeTimeInDays);
            });

            return services;
        }
        public static IServiceCollection RegisterConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityAppSettings>(configuration.GetSection("IdentityAppSettings"));
            services.AddSingleton(configuration);
            return services;
        }
        public static IServiceCollection RegisterIdentityServer(
           this IServiceCollection services,
           IWebHostEnvironment hostingEnvironment,
           IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var authority = configuration.GetSection("Microservices").GetSection("Authority").Value;
            // var migrationsAssembly = GetStartupAssembly();
            //var builder = services.
            //    AddIdentityServer(options =>
            //{
            //    options.Events.RaiseErrorEvents = true;
            //    options.Events.RaiseInformationEvents = true;
            //    options.Events.RaiseFailureEvents = true;
            //    options.Events.RaiseSuccessEvents = true;
            //    options.IssuerUri = authority;
            //    options.EmitStaticAudienceClaim = true;
            //})
            //    .AddAspNetIdentity<ApplicationUser>()
            //    .AddConfigurationStore(options =>
            //    {
            //        options.ConfigureDbContext = b =>
            //            b.UseSqlServer(configuration.GetConnectionString("ConfigurationStore"),
            //                sql => sql.MigrationsAssembly(migrationsAssembly));
            //    })
            //    .AddOperationalStore(options =>
            //    {
            //        options.ConfigureDbContext = b =>
            //            b.UseSqlServer(configuration.GetConnectionString("OperationalStore"),
            //                sql => sql.MigrationsAssembly(migrationsAssembly));

            //        // this enables automatic token cleanup. this is optional.
            //        options.EnableTokenCleanup = false;
            //        // options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
            //    })
            //    .AddProfileService<CustomProfileService<ApplicationUser>>()
            //    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            //    .AddCustomTokenRequestValidator<CustomRequestTokenLifeTimeValidator>();


            //if (hostingEnvironment.IsDevelopment())
            //{
            //    builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    // TODO later set it true for production
            //    builder.AddDeveloperSigningCredential();
            //}


            //services.AddAuthentication()
            //    .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            //    {
            //        options.Authority = authority;
            //        options.ApiName = "IdentityServerApi";
            //        //                    if (hostingEnvironment.IsDevelopment())
            //        //                    {
            //        options.RequireHttpsMetadata = false;
            //        //                    }                        
            //    });
           
            const string connectionStringConfigurationStore = @"Data Source=DESKTOP-2Q2AIBL;Initial Catalog=ConfigurationStore;Integrated Security=False;user id=sa;password=123;MultipleActiveResultSets=true";
            const string connectionStringOperationalStore = @"Data Source=DESKTOP-2Q2AIBL;Initial Catalog=OperationalStore;Integrated Security=False;user id=sa;password=123;MultipleActiveResultSets=true";
            //services.AddDbContext<ApplicationUser>(options =>
            //options.UseSqlServer(connectionStringIdentityServer);
            //  services.RegisterIdentityServer(HostingEnvironment, Configuration);
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
           
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers((List<IdentityServer4.Test.TestUser>)Config.GetUsers())
               .AddAspNetIdentity<ApplicationUser>()
           
           // .AddEntityFrameworkStores<ApplicationDbContext>();
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionStringConfigurationStore,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionStringOperationalStore,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });

            return services;
        }
        private static string GetStartupAssembly()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Development")
            {
                env = "Development";
            }
            var name = "Startup";//Type.GetType($"IdentityServer.Api.Startup").Name;

            return name;
        }
    }
}
