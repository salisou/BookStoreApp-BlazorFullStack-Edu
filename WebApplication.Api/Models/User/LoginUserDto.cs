using System.ComponentModel.DataAnnotations;

namespace WebApplicationApi.Models.User
{
	/// <summary>
	/// DTO (Data Transfer Object) per la login dell'utente, 
	/// utilizzato per raccogliere le informazioni necessarie durante l'autenticazione.
	/// </summary>
	public class LoginUserDto
	{
		/// <summary>
		/// L'indirizzo email dell'utente. Campo obbligatorio.
		/// Deve essere un indirizzo email valido.
		/// </summary>
		[Required(ErrorMessage = "L'indirizzo email è obbligatorio.")]
		[EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
		public string Email { get; set; }

		/// <summary>
		/// La password dell'utente. Campo obbligatorio.
		/// Utilizzata per l'autenticazione.
		/// </summary>
		[Required(ErrorMessage = "La password è obbligatoria.")]
		public string Password { get; set; }
	}
}