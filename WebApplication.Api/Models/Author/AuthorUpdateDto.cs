using System.ComponentModel.DataAnnotations;

namespace WebApplicationApi.Models.Author
{
	/// <summary>
	/// DTO utilizzato per aggiornare le informazioni di un autore esistente
	/// </summary>
	public class AuthorUpdateDto : BaseDto
	{

		// Il nome dell'autore è obbligatorio 
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }
		// Il cognome dell'autore è obbligatorio 
		[Required]
		[StringLength(50)]
		public string LastName { get; set; }
		// La biografia dell'autore è opzionale 
		[StringLength(250)]
		public string Bio { get; set; }
	}
}
