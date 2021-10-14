using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace IdentityServer.Api.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        
        [Required]
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
    }
}