using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.API
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Address(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("AccountAPI", "Account API")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Market Service",
                     Scopes = new List<string>{
                       "AccountAPI"
                    }

                },
                
                 
                
            };
        }
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("api1", "My API")
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                }
            };
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var microservices = configuration.GetSection("Microservices");
            var reverseProxy = microservices.GetSection("ReverseProxy").Value;
            var translationWebApp = microservices.GetSection("TranslationWebApp").Value;
            var account = microservices.GetSection("Account").Value;

            return new List<Client>
            {
                 new Client
                {
                    ClientId = "client",
                    ClientName = "Job Engine",

                    RedirectUris = { $"{account}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{account}/signout-callback-oidc" },

                    ClientSecrets = { new Secret( "secret".Sha256() ) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email"
                       
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RequireConsent = false
                },
            };
        }
        public static IEnumerable<TestUser> GetUsers()
        {

            return new List<TestUser>
            {
                new TestUser
                {
                    Username = "jl@quantumcph.com",
                    Password = "Sh1022",
                    IsActive = true,
                    Claims = new List<Claim>
                    {
                        new Claim("role", "power_user")
                    }
                },
             
            };
        }
    }

}
