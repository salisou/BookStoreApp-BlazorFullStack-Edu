using Microsoft.AspNetCore.Identity;

namespace WebApplicationApi.Dati
{
	/// <summary>
	/// Classe che rappresenta l'utente nel sistema di autenticazione.
	/// Estende IdentityUser per includere informazioni aggiuntive.
	/// </summary>
	public class ApiUser : IdentityUser
	{
		/// <summary>
		/// Nome dell'utente.
		/// </summary>
		public string? FirstName { get; set; }

		/// <summary>
		/// Cognome dell'utente.
		/// </summary>
		public string? LastName { get; set; }
	}
}
