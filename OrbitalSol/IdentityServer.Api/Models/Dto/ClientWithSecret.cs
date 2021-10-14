using IdentityServer4.Models;

namespace IdentityServer.Api.Models.Dto
{
    public class ClientWithSecret
    {
        public Client Client { get; set; }
        public string Secret { get; set; }
    }
}