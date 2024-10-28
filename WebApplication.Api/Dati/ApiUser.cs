using Microsoft.AspNetCore.Identity;

namespace WebApplicationApi.Dati
{
	public class ApiUser : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
	}
}
