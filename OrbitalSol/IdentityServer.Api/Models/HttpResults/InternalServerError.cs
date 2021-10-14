using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Models.HttpResults
{
    public class InternalServerError : ObjectResult
    {
        public InternalServerError(string error) 
            : base(error)
        {            
            StatusCode = 500;
            Value = error;
        }
    }
}