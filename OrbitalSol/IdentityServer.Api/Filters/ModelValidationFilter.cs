using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Library.Helper;

namespace IdentityServer.Api.Filters
{        
    public class ModelValidationFilter : ActionFilterAttribute
    {

        //private readonly ITransformResponse _transformResponse;

        //public ModelValidationFilter(ITransformResponse transformResponse)
        //{
        //    _transformResponse = transformResponse;
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            throw Helper.CreateApiExceptions(context.ModelState.Keys.ToList(), HttpStatusCode.UnprocessableEntity);                                                                                                                             
        }
         
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}