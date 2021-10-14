using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.Library.Configurations;

namespace IdentityServer.Api.Filters
{
    /// <summary>
    /// It adds userId in header with key UserId.
    /// It is to remove dependency on Identity server for private apis.
    /// </summary>
    public class UserDataInHeader : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UserDataInHeader(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
             
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {  
            if(!request.Headers.Contains("UserId"))
                return await base.SendAsync(request, cancellationToken);
            
            if(IsValidGuid(request))
                return await base.SendAsync(request, cancellationToken);
            
            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(HideBoxClaimTypes.Sub)?.Value;
            //if (string.IsNullOrWhiteSpace(userId))
            //    throw Helper.CreateApiException( TranslationKeys.UserNotFound, HttpStatusCode.Unauthorized );
            
            //request.Headers.Remove(CustomHeaders.UserId);
            //request.Headers.Add(CustomHeaders.UserId, userId);            
            return await base.SendAsync(request, cancellationToken);
        }

        private bool IsValidGuid(HttpRequestMessage request)
        {
            try
            {
                var value = request.Headers.FirstOrDefault(i => i.Key == "UserId").Value.FirstOrDefault();
                return Guid.TryParse(value, out _);    
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}