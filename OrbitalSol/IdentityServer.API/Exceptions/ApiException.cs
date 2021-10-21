using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace IdentityServer.API.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public List<string> Errors { get; set; } = new List<string>();
        public int StatusCode { get; set; } = 500;
        public object ApiData { get; set; } = null;
        public bool RequiresTranslation = true;

        public ApiException()
        {

        }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception inner) : base(message, inner)
        {
        }

        public ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = info.GetInt32("StatusCode");
            RequiresTranslation = info.GetBoolean("RequiresTranslation");
            //            Errors = info.GetType();
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("StatusCode", StatusCode);
            info.AddValue("Errors", Errors);
            info.AddValue("RequiresTranslation", RequiresTranslation);
            base.GetObjectData(info, context);
        }
    }
}