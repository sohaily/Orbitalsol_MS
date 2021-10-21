using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.API.Services.Profile
{
    /// <summary>
    /// IProfileService to integrate with ASP.NET Identity.
    /// </summary>    
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <seealso cref="IdentityServer4.Services.IProfileService" />
    public class CustomProfileService<TUser> : IProfileService
        where TUser : class
    {
        private readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;
        private readonly UserManager<TUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomProfileService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        public CustomProfileService(UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                context.IssuedClaims = new List<Claim>();
                return;
            }
            //TODO only add requested claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            context.IssuedClaims = userClaims.ToList();

            //            context.IssuedClaims.AddRange(principal.Claims);
            //            context.AddRequestedClaims(principal.Claims);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}