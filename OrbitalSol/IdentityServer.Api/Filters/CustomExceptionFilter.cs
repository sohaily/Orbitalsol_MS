using System;
using System.Net;
using System.Linq;
using Models.Library.Dto;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.Library.Exceptions;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Api.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await next(context);
            }            
            catch (ApiException e)
            {
                logger.LogCritical(e, $"Message : {e.Message}, InnerMessage : {e.InnerException?.Message}");
                await HandleApiExceptions(context, e);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, $"Message : {e.Message}, InnerMessage : {e.InnerException?.Message}");
                await HandleExceptionAsync(context);
            }
        }
                    
        private Task HandleApiExceptions(HttpContext context, ApiException exception)
        {
            List<string> codes = new List<string>{HttpStatusCode.InternalServerError.ToString()};
            
            if (exception.Errors != null && exception.Errors.Count > 0)            
                    codes = exception.Errors.ToList();                
           
            var translations = codes;                
            var result =  new ApiErrorResponse(translations).ToJson() ;                 
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;
            return context.Response.WriteAsync(result);
        }
                   
        private  Task HandleExceptionAsync(HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;            
            var result =  new ApiErrorResponse( new List<string>(){code.ToString()} ).ToJson();                 
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}