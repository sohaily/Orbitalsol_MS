using Microsoft.Extensions.Configuration;
using IdentityServer.API.Data;
using IdentityServer.API.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                foreach (var testUser in Config.GetUsers())
                {
                    var alice = userMgr.FindByNameAsync(testUser.Username).Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = testUser.Username,
                            Email = testUser.Username,
                            EmailConfirmed = true,
                            CreatedAt = DateTimeOffset.Now
                        };
                        var result = userMgr.CreateAsync(alice, "Sh@1022").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, testUser.Claims.ToList()).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Console.WriteLine($"{testUser.Username} created");
                    }
                    else
                    {
                        Console.WriteLine($"{testUser.Username} already exists");
                    }
                }
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        public static void InitializeConfigurationDatabase(IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();


                foreach (var client in Config.GetClients(configuration))
                {
                   // if (!context.Clients.Any(i => i.ClientId == client.ClientId))
                        context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();

                foreach (var resource in Config.GetIdentityResources())
                {
                    if (!context.IdentityResources.Any(i => i.Name == resource.Name))
                        context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();

                foreach (var resource in Config.GetApiResources())
                {
                    if (!context.ApiResources.Any(i => i.Name == resource.Name))
                        context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
