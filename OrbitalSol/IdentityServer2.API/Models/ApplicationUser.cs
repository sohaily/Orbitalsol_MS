using Microsoft.AspNetCore.Identity;
namespace IdentityServer2.API.Models
{
	public class ApplicationUser : IdentityUser
	{
		public bool IsEnabled { get; set; }
		public string EmployeeId { get; set; }
	}
}
