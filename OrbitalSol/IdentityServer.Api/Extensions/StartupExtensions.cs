using System;
using System.Globalization;
using System.Net.Http.Headers;
using IdentityServer.Api.Data;
using IdentityServer.Api.Models;
using System.Collections.Generic;
using IdentityServer.Api.Filters;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using IdentityServer.Api.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Library.Configurations;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4.AccessTokenValidation;

namespace IdentityServer.Api.Extensions
{
    public static class StartupExtensions
    {
        private const string Notification = "Microservices:NotificationApi";
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
        
        public static bool IsLocalDevelopment(this IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof (hostingEnvironment));
            return hostingEnvironment.IsEnvironment("LocalDevelopment");
        }
         
            
       

        public static IServiceCollection RegisterAwsSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
           
            return services;
        }
        
        public static IServiceCollection Registerpolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy(OrbitalSolPoliciesName.MobileScopePolicy, 
                //    policy => policy.RequireScope(OrbitalSolJwtClaims.MyStuffProfile));
            });
            return services;
        }
        
        public static IServiceCollection RegisterServicesContainer(this IServiceCollection services)
        {
            // Add application services.
            //services.AddTransient<IEmailSender, EmailSender>();
            services.AddHttpContextAccessor();
            services.AddTransient<AddAuthorizationHeaderMiddleware>();
            
           
            services.AddScoped<ModelValidationFilter>();
            services.AddScoped<UserDataFilter>();
            

            services.AddTransient<AddHeaders>();
            services.AddTransient<UserDataInHeader>();
                       
          //  services.AddAutoMapper(typeof(Startup));
            //services.AddSingleton<IResourceOwnerPasswordValidator, 
            //    ResourceOwnerPasswordValidator>();
            return services;
        }
        
        public static IServiceCollection RegisterConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityAppSettings>(configuration.GetSection("IdentityAppSettings"));
           

            services.AddSingleton(configuration);
                        
            var emailApi = configuration.GetSection("Microservices").GetSection("EmailApi").Value;
            //services.AddHttpClient<IEmailClient, EmailClient>(client =>
            //{
            //    client.BaseAddress = new Uri(emailApi);     
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders
            //        .Accept
            //        .Add(new MediaTypeWithQualityHeaderValue("application/json"));   
                
            //});
            
            services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("da-DK")               
                };            
                
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            
            services.AddMemoryCache();
            return services;
        }

        public static IServiceCollection RegisterMvc(this IServiceCollection services)
        {
            services.AddMvc();
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
            var migrationsAssembly = GetStartupAssembly();
            var builder = services.AddIdentityServer(options =>
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
                        b.UseSqlServer(configuration.GetConnectionString("ConfigurationStore"),
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(configuration.GetConnectionString("OperationalStore"),
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = false;
                    // options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });
                //.AddProfileService<CustomProfileService<ApplicationUser>>()
                //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                //.AddCustomTokenRequestValidator<CustomRequestTokenLifeTimeValidator>();
                
            
            if (hostingEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                // TODO later set it true for production
                builder.AddDeveloperSigningCredential();
            }
            
            
            services.AddAuthentication()                
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authority;
                    options.ApiName = ApiResourcesName.IdentityApi;
//                    if (hostingEnvironment.IsDevelopment())
//                    {
                        options.RequireHttpsMetadata = false;
//                    }                        
                });

            return services;
        }

        public static IServiceCollection RegisterMarketClient(this IServiceCollection services, IConfiguration configuration)
        {
            var marketApi = configuration.GetSection("Microservices").GetSection("MarketApi").Value;
            //services.AddHttpClient("MarketRefitClient", client =>
            //{
            //    client.BaseAddress = new Uri(marketApi);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders
            //        .Accept
            //        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //})
            //    .AddTypedClient(client => Refit.RestService.For<IMarketRefitClient>(client))
            //    .AddHttpMessageHandler<AddAuthorizationHeaderMiddleware>()
            //    .AddHttpMessageHandler<AddHeaders>();
            return services;
        }
        public static IServiceCollection RegisterMailChimpSetting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
        //    services.Configure<MailChimpAppSettings>(configuration.GetSection("MailChimpAppSettings"));
            return services;
        }
        
        public static IServiceCollection RegisterMailChimpClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var emailApi = configuration.GetSection("Microservices").GetSection("EmailApi").Value;
            //services.AddHttpClient("IMailChimpRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(emailApi);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(client => Refit.RestService.For<IMailChimpRefitClient>(client));
            return services;
        }

        public static IServiceCollection RegisterMyStuffClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var itemsApi = configuration.GetSection("Microservices").GetSection("ItemsApi").Value;
            //services.AddHttpClient("IMyStuffRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(itemsApi);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(client => Refit.RestService.For<IMyStuffRefitClient>(client))
            //    .AddHttpMessageHandler<AddHeaders>(); ;
            return services;
        }

        public static IServiceCollection RegisterCacheClient(this IServiceCollection services, IConfiguration configuration)
        {
            var itemApi = configuration.GetSection("Microservices").GetSection("CacheApi").Value;

            //services.AddHttpClient("CacheRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(itemApi);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(client => RestService.For<ICacheRefitClient>(client))
            //    .AddHttpMessageHandler<AddHeaders>();

            return services;
        }
        
        public static IServiceCollection RegisterChatClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            var identityApi = configuration
                .GetSection("Microservices")
                .GetSection("ChatApi").Value;
            
            //services.AddHttpClient("ChatRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(identityApi);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(client => Refit.RestService.For<IChatRefitClient>(client));
            
            return services;
        }
        
        public static IServiceCollection RegisterIdentityServerClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            var identityApi = configuration.GetSection("Microservices").GetSection("Authority").Value;
            //services.AddHttpClient("IdentityServerRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(identityApi);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(client => Refit.RestService.For<IIdentityServerRefitClient>(client))
            //    .AddHttpMessageHandler<AddAuthorizationHeaderMiddleware>()
            //    .AddHttpMessageHandler<AddHeaders>();
            return services;
        }
        
        public static IServiceCollection RegisterTranslationClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            var translationApi = configuration.GetSection("Microservices").GetSection("TranslationApi").Value;
            //services.AddHttpClient("TranslationRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(translationApi);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(client => Refit.RestService.For<ITranslationRefitClient>(client))
            //    .AddHttpMessageHandler<AddHeaders>();

            return services;
        }

        public static IServiceCollection RegisterNotificationClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var notificationApi = configuration.GetSection(Notification).Value;
            
            //services.AddHttpClient("NotificationRefitClient", client =>
            //    {
            //        client.BaseAddress = new Uri(notificationApi);     
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders
            //            .Accept
            //            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    })
            //    .AddTypedClient(Refit.RestService.For<INotificationsRefitClient>)
            //    .AddHttpMessageHandler<UserDataInHeader>()
            //    .AddHttpMessageHandler<AddHeaders>();

            return services;
        }
                
        
        private static string GetStartupAssembly()
        {            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "LocalDevelopment")
            {
                env = "Development";
            }
            var name = Type.GetType($"IdentityServer.Api.Startup").Name;

            return name;
        }                
    }
}