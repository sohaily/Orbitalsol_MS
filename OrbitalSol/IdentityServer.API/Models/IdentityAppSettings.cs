using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.Models
{
    public class IdentityAppSettings
    {
        public bool TokenOnSignUp { get; set; } = false;
        public double TokenLifeUnconfirmedEmailInSeconds { get; set; }
        public bool HttpsAllowed { get; set; }
        public int TokenLifeTimeInDays { get; set; } = 365;
        public string ChatApiPassword { get; set; }
        public string UserAvatar { get; set; }
    }
}
