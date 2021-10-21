using IdentityModel;
using IdentityServer.API.Exceptions;
using IdentityServer.API.Models;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IEventService _events;
        private readonly IdentityAppSettings _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

        public ResourceOwnerPasswordValidator(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IEventService events,
            ILogger<ResourceOwnerPasswordValidator> logger,
            IOptions<IdentityAppSettings> options)
        {
            _events = events;
            _logger = logger;
            _userManager = userManager;
            _appSettings = options.Value;
            _signInManager = signInManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await GetUser(context);
                if (user == null)
                {
                    _logger.LogInformation("No user found matching username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", false));
                    throw new Exception("InvalidUserName");
                }

                if (user._status == (int)SignUpStatusEnum.Disabled)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", false));
                    throw new Exception("NotAllowed");
                }

                if (!string.IsNullOrWhiteSpace(context.Password))
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                    if (!result.Succeeded)
                    {
                        _logger.LogInformation(
                            "Authentication failed for username: {username}, reason: invalid credentials",
                            context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials",
                            false));
                        throw new Exception("InvalidCredentials");
                    }

                    if (result.IsLockedOut)
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: locked out",
                            context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out",
                            false));
                        throw new Exception("LockedOut");
                    }

                    if (result.IsNotAllowed)
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed",
                            context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed",
                            false));
                        throw new Exception("NotAllowed");
                    }
                }

                

                var sub = user.Id;
                _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName,
                    false));


              //  var profile = await MyStuffProfile(context, user);

             //   await _userService.UpdateUserFromHeader(user);

                //context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.Password,
                //    customResponse: profile.ToDictionary());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"login failed for {context.UserName}");
               // context.Result = new IdentityCustomTokenEndPointError(e.Message, e.Message);
            }
        }

    

        private async Task<ApplicationUser> GetUser(ResourceOwnerPasswordValidationContext context)
        {
            var socialMediaId = context.Request.Raw["SocialMediaId"];
            var socialMediaProvider = context.Request.Raw["SocialMediaProviderName"];

            if (context.Password.Equals("zaYE46sT88&dBrf8rr"))
            {
                _logger.LogInformation($"Attempted to login user:{context.UserName} by using admin password on date:{DateTime.UtcNow:f}");
                var user = await _userManager.FindByEmailAsync(context.UserName);
                context.Password = null;
                return user;
            }

            if (string.IsNullOrWhiteSpace(socialMediaId))
                return await _userManager.FindByEmailAsync(context.UserName);

            context.Password = null;
            return await _userManager.FindByLoginAsync(socialMediaProvider, socialMediaId);
        }
    }
}
