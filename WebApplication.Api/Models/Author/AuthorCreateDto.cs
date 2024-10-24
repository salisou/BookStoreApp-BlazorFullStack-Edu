using System.ComponentModel.DataAnnotations;

namespace WebApplicationApi.Models.Author
{
	/// <summary>
	/// DTO (Data Transfer Object) utilizzato per la creazione di nuovi autori
	/// </summary>
	public class AuthorCreateDto
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

