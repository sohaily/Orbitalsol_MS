using System;

namespace IdentityServer.Api.Models.Emails
{
    public class ResetSecurityPinEmailDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public GoogleAddress Googleaddress { get; set; }
        public string Time { get; set; } = DateTime.Now.ToShortDateString();
        public string IpAddress { get; set; } = "";
        public string SecurityPinEmailType { get; set; } 
    }
    
    public class ResetSecurityPinDto
    {
        public GoogleAddress Googleaddress { get; set; }
        public string IpAddress { get; set; }
        public PinEmailType SecurityPinEmailType { get; set; }
    }

    public class GoogleAddress
    {
        public string Longitude { set; get; }
        public string Latitude { set; get; }
        public string Address { set; get; }
    }

    public enum PinEmailType
    {
        Change=1,
        Retry=2,
        Forgot=3
    }
    
}