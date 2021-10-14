using IdentityServer4.Validation;

namespace IdentityServer.Api.Models.Dto
{
    public class IdentityCustomTokenEndPointError : GrantValidationResult
    {
        public IdentityCustomTokenEndPointError(string error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;    
            IsError = true;            
        }
    }
}