using Microsoft.AspNetCore.Mvc;
using Models.Library.Helper;

namespace IdentityServer.Api.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, 
            string userId, 
            string code, 
            string scheme,
            string culture,
            string host)
        {
            return urlHelper.Action(
                action: "EmailConfirmed",
                controller: "Account",
                values: new { userId, code, culture },
                host : Helper.GetHost(host), 
                protocol: scheme);
        }
        
        //public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, 
        //    string code, 
        //    string scheme,
        //    string culture,
        //    string host)
        //{            
        //    return urlHelper.Action(
        //        action: nameof(AccountController.ResetPassword),
        //        controller: "Account",
        //        values: new { userId, code, culture },
        //        host : Helper.GetHost(host),
        //        protocol: scheme);
        //}

        //public static string ResetPinCallbackLink(this IUrlHelper urlHelper, string userId,
        //    string scheme,
        //    string culture,
        //    string host)
        //{
        //    return urlHelper.Action(
        //        action: nameof(AccountController.ResetPin),
        //        controller: "Account",
        //        values: new { userId, culture },
        //        host: Helper.GetHost(host),
        //        protocol: scheme);
        //}
    }
}
