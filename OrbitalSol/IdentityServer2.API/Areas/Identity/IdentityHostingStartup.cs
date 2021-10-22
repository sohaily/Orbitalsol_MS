using System;
using IdentityServer2.API.Data;
using IdentityServer2.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityServer2.API.Areas.Identity.IdentityHostingStartup))]
namespace IdentityServer2.API.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
             builder.ConfigureServices((context, services) => {

             });
            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<IdentityDbContext>(options =>
            //        options.UseSqlServer(
            //            context.Configuration.GetConnectionString("IdentityDbContextConnection")));

            //    services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddEntityFrameworkStores<IdentityDbContext>();
            //});
        }
    }
}