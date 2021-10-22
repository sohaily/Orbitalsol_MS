using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer2.API
{
    public class Program
    {
            public static void Main(string[] args)
            {
                Console.Title = "IdentityServer4";

                var host = CreateWebHostBuilder(args).Build();

                var config = host.Services.GetRequiredService<IConfiguration>();
                bool seed = config.GetSection("Data").GetValue<bool>("Seed");
                if (seed)
                {
                    var connectionString = config.GetConnectionString("DefaultConnection");
                    Users.EnsureSeedData(connectionString);
                }

                host.Run();
            }

            public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            {
                return WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseSerilog((context, configuration) =>
                        {
                            configuration
                                .MinimumLevel.Debug()
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                .MinimumLevel.Override("System", LogEventLevel.Warning)
                                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                                .Enrich.FromLogContext()
                                .WriteTo.File(@"identityserver4_log.txt")
                                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                        });
            }
        }
    }
