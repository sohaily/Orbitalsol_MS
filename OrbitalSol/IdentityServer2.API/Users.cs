using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer2.API.Data;
using IdentityServer2.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer2.API
{
    public class Users
    {
       public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IdentityDbContext>();

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
                           // CreatedAt = DateTimeOffset.Now
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

    }
}
