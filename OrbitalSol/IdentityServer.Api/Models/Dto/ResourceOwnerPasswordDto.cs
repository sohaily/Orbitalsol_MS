namespace IdentityServer.Api.Models.Dto
{
    public class ResourceOwnerPasswordDto
    {
        public string ClientId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string SocialMediaProviderName { get; set; }
        public string SocialMediaId { get; set; }
        public string LanguageCode { get; set; }
        public string UserAgent { get; set; }
    }
}