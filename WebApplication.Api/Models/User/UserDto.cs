using System.ComponentModel.DataAnnotations;

namespace WebApplicationApi.Models.User
{
	/// <summary>
	/// DTO (Data Transfer Object) per l'utente, usato durante la registrazione.
	/// Estende LoginUserDto per aggiungere ulteriori informazioni.
	/// </summary>
	public class UserDto : LoginUserDto
	{
		/// <summary>
		/// Nome dell'utente. Campo obbligatorio.
		/// </summary>
		[Required(ErrorMessage = "Il campo 'Nome' è obbligatorio.")]
		public string FirstName { get; set; }

		/// <summary>
		/// Cognome dell'utente. Campo obbligatorio.
		/// </summary>
		[Required(ErrorMessage = "Il campo 'Cognome' è obbligatorio.")]
		public string LastName { get; set; }

		/// <summary>
		/// Ruolo dell'utente nel sistema (es. Amministratore, Utente). Campo obbligatorio.
		/// </summary>
		[Required(ErrorMessage = "Il campo 'Ruolo' è obbligatorio.")]
		public string Role { get; set; }
	}
}
