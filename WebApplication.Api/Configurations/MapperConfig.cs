using AutoMapper;
using WebApplicationApi.Dati;
using WebApplicationApi.Models.Author;

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
			CreateMap<AuthorUpdateDto, Author>().ReverseMap();
			CreateMap<AuthorReadOlnlyDto, Author>().ReverseMap();
		}
	}
}
