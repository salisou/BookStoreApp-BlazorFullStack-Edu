using AutoMapper;
using WebApplicationApi.Dati;
using WebApplicationApi.Models.Author;
using WebApplicationApi.Models.Book;

namespace WebApplicationApi.Configurations
{
	/// <summary>
	/// Classe di configurazione per AutoMapper, 
	/// utilizzata per definire le mappature tra DTO e modelli del dominio
	/// </summary>
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			// Definisce una mappatura bidirezionale tra AuthorCreateDto e Author
			CreateMap<AuthorCreateDto, Author>().ReverseMap();
			// Definisce una mappatura bidirezionale tra AuthorUpdateDto e Author
			CreateMap<AuthorUpdateDto, Author>().ReverseMap();
			// Definisce una mappatura bidirezionale tra AuthorReadOlnlyDto e Author
			CreateMap<AuthorReadOlnlyDto, Author>().ReverseMap();


			// Mappatura tra il modello Book e il DTO BookReadOnlyDto.
			// BookReadOnlyDto rappresenta i dati essenziali per la lettura di un libro (ID, Titolo, Prezzo, Nome Autore).
			CreateMap<Book, BookReadOnlyDto>()
				// Configura il campo AuthorName in BookReadOnlyDto combinando i campi FirstName e LastName dell'autore.
				.ForMember(q => q.AuthorName,
						   d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

			// Mappatura tra il modello Book e il DTO BookDetailsDto.
			// BookDetailsDto è usato per mostrare i dettagli completi di un libro, inclusi i dettagli dell'autore.
			CreateMap<Book, BookDetailsDto>()
				// Configura il campo AuthorName in BookDetailsDto combinando i campi FirstName e LastName dell'autore.
				.ForMember(q => q.AuthorName,
						   d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

			// Mappatura tra il DTO BookCreateDto e il modello Book.
			// BookCreateDto rappresenta i dati necessari per la creazione di un nuovo libro (Titolo, ISBN, Prezzo, ecc.).
			CreateMap<BookCreateDto, Book>().ReverseMap();

			// Mappatura tra il DTO BookUpdateDto e il modello Book.
			// BookUpdateDto rappresenta i dati necessari per l'aggiornamento di un libro esistente (ID, Titolo, Prezzo, ecc.).
			CreateMap<BookUpdateDto, Book>().ReverseMap();

		}
	}
}
