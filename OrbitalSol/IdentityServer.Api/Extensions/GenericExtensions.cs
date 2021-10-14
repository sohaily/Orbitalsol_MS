using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using IdentityServer.Api.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Models.Library.Configurations;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace IdentityServer.Api.Extensions
{
    public static class GenericExtensions
    {
        public static string GetToken(this IHttpContextAccessor contextAccessor)
        {
            try
            {
                var token = contextAccessor.HttpContext.Request.Headers["Authorization"];
                return token.ToString().Split(" ")[1];
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static string GetTokenType(this IHttpContextAccessor contextAccessor)
        {
            try
            {
                var token = contextAccessor.HttpContext.Request.Headers["Authorization"];
                return token.ToString().Split(" ")[0];
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetHeaderValue(this HttpContext httpContext, string header)
        {
            if (string.IsNullOrWhiteSpace(header))
                return null;
            
            var result = httpContext.Request.Headers[header];
            return string.IsNullOrWhiteSpace(result) ? null : result.ToString();
        }
        
        public static double GetDateTimeDiffFromNowInSeconds(this DateTimeOffset dateTime)
        {
            return (DateTimeOffset.Now - dateTime).TotalSeconds;
        }
             
        //public static Dictionary<string, object> ToDictionary(this ProfileDtoToReturn user)
        //{                                          
        //    var properties = user.GetType()
        //        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        //    if (!properties.Any()) return null;
            
        //    Dictionary<string, object> claims = new Dictionary<string, object>();
        //    claims.Add( "ProfileImage", user.ProfileImage );
        //    claims.Add( "CoverImage", user.CoverImage );
        //    claims.Add( "Location", user.Location );
            
        //    foreach (var propertyInfo in properties)
        //    {   
        //        if(propertyInfo.Name == "ProfileImage" || propertyInfo.Name == "CoverImage" || propertyInfo.Name == "Location")
        //            continue;
                
        //        var value = propertyInfo.GetValue(user, null);
        //        if( value == null )
        //            continue;
                                
        //        claims.Add( propertyInfo.Name, value.ToString() );                
        //    }
            
        //    return claims;
        //}
        
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                    .GetProperty(item.Key)
                    .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static string GetUserName(this ApplicationUser user)
        {
            var name = $"{user.FirstName} {user.LastName}";
            return string.IsNullOrWhiteSpace(name) ? user.Email : name;
        }
        
        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static Guid GetUserIdAsGuid(this HttpContext httpContext)
        {
            var userId = httpContext
                .User
                .Claims.FirstOrDefault(x => x.Type.Equals(JwtClaimTypes.Subject))?.Value;

            return !string.IsNullOrWhiteSpace(userId)
                ? new Guid(userId)
                : Guid.Empty;
        }
    }
}