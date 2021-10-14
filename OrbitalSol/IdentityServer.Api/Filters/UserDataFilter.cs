using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Api.Models;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Library.Dto;

namespace IdentityServer.Api.Filters
{
    public class UserDataFilter : IAsyncActionFilter
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ITransformResponse _transformResponse;
    
        public UserDataFilter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            /*Changes due to .NET Core 3.1 upgradation. Authorize is treated as attribute now rather then filter.*/
            var isAuthorizeFilterExist = context.ActionDescriptor.EndpointMetadata.Any(i => i.GetType().Name == "AuthorizeAttribute");
                //context.Filters.Any(i => i.GetType().Name == "AuthorizeFilter");
            if (isAuthorizeFilterExist)
            {
                var userId = context.HttpContext.User.GetSubjectId();    
                var user = await _userManager.FindByIdAsync(userId);
                context.HttpContext.Items["User"] = user;
                //if (user == null)
                //{
                //    context.Result = _transformResponse.TransformErrorResponse( TranslationKeys.UserNotFound, 400 );
                //}                                                    
            }
            
            await next();
        }
    }
}