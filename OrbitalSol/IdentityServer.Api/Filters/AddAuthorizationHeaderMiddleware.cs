using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Api.Extensions;
using IdentityServer4;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Api.Filters
{
    public class AddAuthorizationHeaderMiddleware : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityServerTools _identityServerTools;
        public AddAuthorizationHeaderMiddleware(IHttpContextAccessor httpContextAccessor, IdentityServerTools identityServerTools)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityServerTools = identityServerTools;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

            if (_httpContextAccessor.HttpContext.Request.Headers != null && request.Headers.Contains("Authorization") &&
                    _httpContextAccessor.HttpContext.Request.Headers["Authorization"] != string.Empty)
            {

                var token = _httpContextAccessor.GetToken();
                var tokenType = _httpContextAccessor.GetTokenType();
                
                if (!string.IsNullOrEmpty(token) && tokenType.ToLower() != "basic")

                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    var selfToken = await _identityServerTools.IssueClientJwtAsync(clientId: "resourceowner", lifetime: 3600, 
                        audiences: new[] { "marketapi" });
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", selfToken);
                }
            }

            if (_httpContextAccessor.HttpContext.Request.Headers != null &&
                    _httpContextAccessor.HttpContext.Request.Headers["GuestAuth"] != string.Empty)
            {

                var token = _httpContextAccessor.HttpContext.Request.Headers["GuestAuth"];
                if (token.Count > 0)
                {
                    request.Headers.Add("GuestAuth", token.ToString());
                    request.Headers.Remove("Authorization");
                }


            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}