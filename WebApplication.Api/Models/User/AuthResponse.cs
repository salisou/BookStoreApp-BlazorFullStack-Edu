namespace WebApplicationApi.Models.User
{
	/// <summary>
	/// Modello per la risposta di autenticazione, 
	/// utilizzato per restituire all'utente le informazioni essenziali dopo il login.
	/// </summary>
	public class AuthResponse
	{
		/// <summary>
		/// L'ID dell'utente autenticato, usato per identificare l'utente nel sistema.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Il token JWT (JSON Web Token) generato per l'utente, 
		/// utilizzato per l'autenticazione e l'autorizzazione alle risorse protette.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// L'indirizzo email dell'utente autenticato.
		/// Utile per identificare l'utente e per la conferma visiva.
		/// </summary>
		public string Email { get; set; }
	}
}