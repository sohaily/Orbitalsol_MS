using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Library.Configurations
{
  
        public static class OrbitalSolJwtClaims
    {
            public const string PowerUser = "power_user";
            public const string Developer = "developer";
            public const string MyStuffProfile = "my_stuff_profile";
        }

        public static class ApiResourcesName
        {
            public const string MyStuffApi = "MyStuffApiService";
            public const string IdentityApi = "IdentityServerApi";
            public const string ReverseProxy = "reverseproxyservice";
            public const string Translations = "translationsapiaccessscheme";
            public const string MarketApi = "marketapi";
            public const string CacheApi = "CacheApi";
            public const string AuctionApi = "AuctionApi";
            public const string FeedApi = "FeedApi";
            public const string JobEngineApi = "JobEngineApi";
        }

        public static class OrbitalSolClaimsScopes
    {
            public const string Gender = "Gender";
            public const string Country = "country";
            public const string Website = "Website";
            public const string AboutMe = "AboutMe";
            public const string LastName = "LastName";
            public const string BirthDay = "BirthDay";
            public const string FirstName = "FirstName";
            public const string CreatedAt = "CreatedAt";
            public const string PhoneNumber = "PhoneNumber";
            public const string MaritalStatus = "MaritalStatus";
            public const string EmailConfirmed = "EmailConfirmed";
            public const string IsSocialMediaUser = "IsSocialMediaUser";
        }

        public static class OrbitalSolClients
    {
            public const string ResourceOwnerClient = "resourceowner";
            public const string TranslationClient = "translationclient";
            public const string ReverseProxyClient = "reverseproxyservice";
            public const string SwaggerClient = "rp";
            public const string WebClient = "hideboxweb";
            public const string CacheApi = "CacheApi";
            public const string JobEngine = "JobEngine";
        }


    }
