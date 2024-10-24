namespace WebApplicationApi.Models.Author
{
	/// <summary>
	/// DTO utilizzato per rappresentare un autore
	/// </summary>
	public class AuthorReadOlnlyDto : BaseDto
	{
		// Nome dell'autore
		public string FirstName { get; set; }

		// Cognome dell'autore
		public string LastName { get; set; }

		// Biografia dell'autore
		public string Bio { get; set; }
	}
}
