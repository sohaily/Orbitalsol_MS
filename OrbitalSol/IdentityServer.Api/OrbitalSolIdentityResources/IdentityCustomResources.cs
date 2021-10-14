using IdentityModel;
using IdentityServer4.Models;
using Models.Library.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Api.OrbitalSolIdentityResources
{
    public static class IdentityCustomResources
    {
        public class MyStuffProfile : IdentityResource
        {
            public MyStuffProfile()
            {
                Name = OrbitalSolJwtClaims.MyStuffProfile;
                Description = "User Profile Information related to My Stuff";
                DisplayName = "User Profile Information related to My Stuff";
                Enabled = true;
                ShowInDiscoveryDocument = true;

                UserClaims.Add(OrbitalSolClaimsScopes.Country);
                UserClaims.Add(OrbitalSolClaimsScopes.FirstName);
                UserClaims.Add(OrbitalSolClaimsScopes.LastName);
                UserClaims.Add(OrbitalSolClaimsScopes.PhoneNumber);
                UserClaims.Add(OrbitalSolClaimsScopes.CreatedAt);
                UserClaims.Add(OrbitalSolClaimsScopes.BirthDay);
                UserClaims.Add(OrbitalSolClaimsScopes.EmailConfirmed);
                UserClaims.Add(OrbitalSolClaimsScopes.Gender);
                UserClaims.Add(OrbitalSolClaimsScopes.MaritalStatus);
                UserClaims.Add(OrbitalSolClaimsScopes.Website);
                UserClaims.Add(OrbitalSolClaimsScopes.AboutMe);
                UserClaims.Add(OrbitalSolClaimsScopes.IsSocialMediaUser);
            }
        }

        public class MyStuffRole : IdentityResource
        {
            public MyStuffRole()
            {
                Name = JwtClaimTypes.Role;
                Description = "User role";
                DisplayName = "Role";
                Enabled = true;
                ShowInDiscoveryDocument = true;

                UserClaims.Add(JwtClaimTypes.Role);
            }
        }
    }
}