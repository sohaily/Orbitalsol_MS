// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Api.OrbitalSolIdentityResources;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using Models.Library.Configurations;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Api
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityCustomResources.MyStuffProfile(),
                new IdentityCustomResources.MyStuffRole(),
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
                new ApiResource(ApiResourcesName.MarketApi, "Market Service")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Market Service",
                     Scopes = new List<string>{
                        ApiResourcesName.MarketApi
                    }

                },
                new ApiResource(ApiResourcesName.Translations, "Translation Service")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Translation Service",
                      Scopes = new List<string>{

                        ApiResourcesName.Translations
                    }
                },
                new ApiResource(ApiResourcesName.ReverseProxy, "Reverse Proxy Service")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Reverse Proxy Service",
                    Scopes = new List<string>{

                        ApiResourcesName.ReverseProxy
                    }
                },
                new ApiResource(ApiResourcesName.IdentityApi, "User Profile, Registration and settings api.")
                {
                    Enabled = true,
                    Description = "This is just proxy server",
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                      Scopes = new List<string>{

                        ApiResourcesName.IdentityApi
                    }
                },
                new ApiResource(ApiResourcesName.MyStuffApi, "My Stuff Insurance and items")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "My Stuff Insurance and items",
                      Scopes = new List<string>{

                        ApiResourcesName.MyStuffApi
                    }
                } ,
                new ApiResource(ApiResourcesName.CacheApi, "Cache Api")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Cache Api" ,
                    Scopes = new List<string>{

                        ApiResourcesName.CacheApi
                    }
                },
                 new ApiResource(ApiResourcesName.AuctionApi, "Auction Api")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Auction Api",
                    Scopes = new List<string>{

                        ApiResourcesName.AuctionApi
                    }
                },
                 new ApiResource(ApiResourcesName.FeedApi, "Feed Api")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "Feed Api",
                    Scopes = new List<string>{

                        ApiResourcesName.FeedApi
                    }
                },
                 new ApiResource(ApiResourcesName.JobEngineApi, "JobEngine Api")
                {
                    ApiSecrets = new List<Secret>{ new Secret("secret") },
                    Enabled = true,
                    Description = "JobEngine Api",
                     Scopes = new List<string>{

                        ApiResourcesName.JobEngineApi
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var microservices = configuration.GetSection("Microservices");
            var reverseProxy = microservices.GetSection("ReverseProxy").Value;
            var translationWebApp = microservices.GetSection("TranslationWebApp").Value;
            var jobEngine = microservices.GetSection("JobEngine").Value;

            return new List<Client>
            {
                new Client
                {
                    ClientId = OrbitalSolClients.SwaggerClient,
                    ClientName = "Swagger UI for Reverse Proxy",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{reverseProxy}/swagger/ui/oauth2-redirect.html" },
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email",
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.Translations,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.CacheApi,
                        ApiResourcesName.MarketApi,
                        ApiResourcesName.AuctionApi,
                        ApiResourcesName.FeedApi,
                        ApiResourcesName.JobEngineApi
                    }
                },
                new Client
                {
                    ClientId = OrbitalSolClients.TranslationClient,
                    ClientName = "Translation Api Client",

                    RedirectUris = { $"{translationWebApp}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{translationWebApp}/signout-callback-oidc" },

                    ClientSecrets = { new Secret( "secret".Sha256() ) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email",
                        ApiResourcesName.Translations,
                        new IdentityCustomResources.MyStuffRole().Name,
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.CacheApi,
                        ApiResourcesName.MarketApi,
                        ApiResourcesName.FeedApi,
                        ApiResourcesName.JobEngineApi
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId = OrbitalSolClients.ReverseProxyClient,
                    ClientName = "Reverse Proxy Client",
                    ClientSecrets = { new Secret("2PC#raJqQ".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = {
                        "openid",
                        "profile",
                        "email",
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.Translations,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.CacheApi
                    },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                new Client
                {
                    ClientId = OrbitalSolClients.ResourceOwnerClient,
                    ClientName = "Resource Owner Client",
                    RequireClientSecret  = true,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        "openid",
                        "profile",
                        "email",
                        "offline_access",
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.Translations,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.MarketApi,
                        ApiResourcesName.CacheApi,
                        ApiResourcesName.AuctionApi,
                        ApiResourcesName.FeedApi,
                        ApiResourcesName.JobEngineApi
                    },
                    AccessTokenLifetime = (int)TimeSpan.FromHours(6).TotalSeconds,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 0,
                    SlidingRefreshTokenLifetime = (int)TimeSpan.FromDays(365).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                new Client
                {
                    ClientId = OrbitalSolClients.CacheApi,
                    ClientName = "Cache Api",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{reverseProxy}/swagger/ui/oauth2-redirect.html" },
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email",
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.Translations,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.CacheApi
                    }
                },
                new Client
                {
                    ClientId = OrbitalSolClients.WebClient,
                    ClientName = "Hide box Web",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{reverseProxy}/swagger/ui/oauth2-redirect.html" },
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email",
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.Translations,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.CacheApi,
                        ApiResourcesName.MarketApi
                    }
                },
                 new Client
                {
                    ClientId = OrbitalSolClients.JobEngine,
                    ClientName = "Job Engine",

                    RedirectUris = { $"{jobEngine}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{jobEngine}/signout-callback-oidc" },

                    ClientSecrets = { new Secret( "secret".Sha256() ) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "email",
                        ApiResourcesName.Translations,
                        new IdentityCustomResources.MyStuffRole().Name,
                        OrbitalSolJwtClaims.MyStuffProfile,
                        ApiResourcesName.ReverseProxy,
                        ApiResourcesName.IdentityApi,
                        ApiResourcesName.MyStuffApi,
                        ApiResourcesName.CacheApi,
                        ApiResourcesName.MarketApi,
                        ApiResourcesName.FeedApi,
                        ApiResourcesName.JobEngineApi,
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RequireConsent = false
                },
            };
        }

        public static IEnumerable<IdentityServer4.Test.TestUser> GetUsers()
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
                        new Claim("role", OrbitalSolJwtClaims.PowerUser)
                    }
                },
                new TestUser
                {
                    Username = "sha@quantumcph.com",
                    Password = "Sa1022",
                    IsActive = true,
                    Claims = new List<Claim>
                    {
                        new Claim("role", OrbitalSolJwtClaims.Developer)
                    }
                },
                new TestUser
                {
                    Username = "sak@quantumcph.com",
                    Password = "Sh1022",
                    IsActive = true,
                    Claims = new List<Claim>
                    {
                        new Claim("role", OrbitalSolJwtClaims.Developer)
                    }
                },
                new TestUser
                {
                    Username = "azr@quantumcph.com",
                    Password = "Sh1022",
                    IsActive = true,
                    Claims = new List<Claim>
                    {
                        new Claim("role", OrbitalSolJwtClaims.Developer)
                    }
                },
                new TestUser
                {
                    Username = "haf@quantumcph.com",
                    Password = "Sh1022",
                    IsActive = true,
                    Claims = new List<Claim>
                    {
                        new Claim("role", OrbitalSolJwtClaims.Developer)
                    }
                }
            };
        }
    }
}
