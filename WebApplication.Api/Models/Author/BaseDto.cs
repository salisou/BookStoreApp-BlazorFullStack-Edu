namespace WebApplicationApi.Models.Author
{
	/// <summary>
	/// Classe astratta di base per i Data Transfer Object (DTO) che contiene l'Id comune
	/// </summary>
	public abstract class BaseDto
	{
		// Proprietà comune che rappresenta l'identificativo unico (Id) di ogni entità
		public int Id { get; set; }
	}
}