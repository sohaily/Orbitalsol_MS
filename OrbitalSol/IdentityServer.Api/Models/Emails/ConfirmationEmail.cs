namespace IdentityServer.Api.Models.Emails
{
    public class ConfirmationEmail
    {
        public string Email { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
    }
    
}