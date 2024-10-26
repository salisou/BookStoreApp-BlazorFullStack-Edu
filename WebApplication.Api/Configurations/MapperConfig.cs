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


			CreateMap<Book, BookReadOnlyDto>()
				.ForMember(q => q.AuthorName,
						   d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

			CreateMap<Book, BookDetailsDto>()
				.ForMember(q => q.AuthorName,
						   d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

			CreateMap<BookCreateDto, Book>().ReverseMap();

			CreateMap<BookUpdateDto, Book>().ReverseMap();
		}
	}
}
