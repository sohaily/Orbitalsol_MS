using IdentityServer.API.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.Services
{
    public class CustomRequestTokenLifeTimeValidator : ICustomTokenRequestValidator
    {
      //  private readonly IEmailConfirmationChecker _emailConfirmationChecker;
        private readonly IdentityAppSettings _appSettings;
        private const string ClientName = "resourceowner";
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomRequestTokenLifeTimeValidator(
            IOptions<IdentityAppSettings> appSettings,
            UserManager<ApplicationUser> userManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }

        public async Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var request = context.Result.ValidatedRequest;

            var subject = request.Subject ?? request.AuthorizationCode?.Subject;

            if (subject != null && request.Client.ClientId == ClientName)
            {
                var sub = subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                var isVerified = user.EmailConfirmed;
                //var isVerified = _emailConfirmationChecker.IsVerified(context);
                if (!isVerified)
                {
                    // User will get the remaining time token if Email is not confirmed yet or it will not be provided with the new token.
                  //  var difference = GetDateTimeDiffFromNowInSeconds();
                    if (!user.EmailConfirmed)
                    {
                        request.AccessTokenLifetime = (int)(_appSettings.TokenLifeUnconfirmedEmailInSeconds);
                    }
                    else
                    {
                        context.Result.Error = "Not authorize";// TranslationKeys.TwentyFourHoursPassed;
                        context.Result.ErrorDescription = "Not authorize";//TranslationKeys.TwentyFourHoursPassed;
                        context.Result.IsError = true;
                    }
                }
            }
        }

        private double GetDateTimeDiffFromNowInSeconds(DateTimeOffset dateTime)
        {
            return (DateTimeOffset.Now - dateTime).TotalSeconds;
        }

       
    }
}