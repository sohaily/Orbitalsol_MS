using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Models.HttpResults
{
    public class ClientErrors : ObjectResult
    {
        public ClientErrors(string message, int statusCode) 
            : base( message  )
        {
            StatusCode = statusCode;    
        }
    }
}