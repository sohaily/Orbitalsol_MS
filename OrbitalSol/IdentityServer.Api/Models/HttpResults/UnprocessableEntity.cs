using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityServer.Api.Models.HttpResults
{
    public class UnprocessableEntity : ObjectResult
    {
        public UnprocessableEntity(ModelStateDictionary modelStateDictionary) 
            : base(new SerializableError(modelStateDictionary))
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException( nameof(modelStateDictionary) );
            }

            StatusCode = 422;   
        }
    }
}