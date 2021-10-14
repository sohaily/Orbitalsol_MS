using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Models.Library.Configurations;
using Models.Library.Helper;

namespace IdentityServer.Api.Filters
{
    public class AddHeaders : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddHeaders(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            //{
            //    if (_httpContextAccessor.HttpContext.Request.Headers != null &&
            //        _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.AcceptLanguage] != string.Empty)
            //    {
            //        var acceptLanguage = GetHeaderValue(HeaderNames.AcceptLanguage);
            //        var langString = Helper.GetSupportedCultureString(acceptLanguage);
            //        request.Headers.AcceptLanguage.Clear();
            //        request
            //            .Headers
            //            .Add(HeaderNames.AcceptLanguage, langString);
            //    }

            //    if (_httpContextAccessor.HttpContext.Request.Headers != null &&
            //        !_httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString().IsNullOrWhiteSpace())
            //    {
            //        var userAgent = GetHeaderValue(HeaderNames.UserAgent);
            //        request.Headers.UserAgent.Clear();
            //        request
            //            .Headers
            //            .Add(HeaderNames.UserAgent, userAgent);
            //    }

            //    if (_httpContextAccessor.HttpContext.Request.Headers != null &&
            //        _httpContextAccessor.HttpContext.Request.Headers[CustomHeaders.Application.AppVersion] != string.Empty)
            //    {
            //        var appVersion = GetHeaderValue(CustomHeaders.Application.AppVersion);
            //        request
            //            .Headers
            //            .Add(CustomHeaders.Application.AppVersion, appVersion);
            //    }
            //}
            return await base.SendAsync(request, cancellationToken);
        }

        private string GetHeaderValue(string headerName)
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers[headerName];
            if (!header.Any())
                return "";

            return header.ToString();
        }

    }
}