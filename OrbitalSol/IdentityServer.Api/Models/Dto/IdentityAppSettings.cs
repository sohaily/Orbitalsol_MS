namespace IdentityServer.Api.Models.Dto
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