using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Dati;
using WebApplicationApi.Models.Author;
using WebApplicationApi.Staic;

namespace WebApplicationApi.Controllers
{
	// Controller che gestisce le operazioni CRUD (Create, Read, Update, Delete) degli Autori
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorsController : ControllerBase
	{
		private readonly BookStoreDbContext _context;
		private readonly IMapper mapper;
		private readonly ILogger<AuthorsController> logger;

		// Costruttore del controller: riceve il contesto del database, il mapper per AutoMapper e il logger per il logging delle operazioni
		public AuthorsController(BookStoreDbContext context, IMapper _mapper, ILogger<AuthorsController> _logger)
		{
			_context = context;
			this.mapper = _mapper;
			logger = _logger;
		}

		/// <summary>
		/// Ottiene una lista di tutti gli autori.
		/// </summary>
		/// <returns>Una lista di autori.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AuthorReadOlnlyDto>>> GetAuthors()
		{
			logger.LogInformation($"Richiesta a {nameof(GetAuthors)}👤");
			try
			{
				var listaAutori = await _context.Authors.ToListAsync();
				var authorDto = mapper.Map<IEnumerable<AuthorReadOlnlyDto>>(listaAutori);
				return Ok(authorDto);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante il recupero in {nameof(GetAuthors)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// Ottiene un autore specifico in base all'ID
		/// </summary>
		/// <param name="id">L'ID dell'autore da recuperare.</param>
		/// <returns>L'autore corrispondente all'ID fornito.</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<AuthorReadOlnlyDto>> GetAuthorById(int id)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);

				if (author == null)
				{
					logger.LogWarning($"Record non trovato: {nameof(GetAuthorById)} - ID {id}");
					return NotFound();
				}

				var authorDto = mapper.Map<AuthorReadOlnlyDto>(author);
				return Ok(authorDto);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante il recupero in {nameof(GetAuthors)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// Aggiorna un autore esistente.
		/// </summary>
		/// <param name="id">L'ID dell'autore da aggiornare.</param>
		/// <param name="authorDto">I dati dell'autore aggiornati.</param>
		/// <returns>Risultato dell'operazione di aggiornamento.</returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
		{
			if (id != authorDto.Id)
			{
				logger.LogWarning($"ID di aggiornamento non valido in: {nameof(PutAuthor)} - ID {id} 😭");
				return BadRequest();
			}

			var author = await _context.Authors.FindAsync(id);

			if (author == null)
			{
				logger.LogWarning($"{nameof(Author)} Record non trovato: {nameof(PutAuthor)} - ID {id} 😭");
				return NotFound();
			}

			mapper.Map(authorDto, author);
			_context.Entry(author).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				if (!await AuthorExists(id))
				{
					return NotFound();
				}
				else
				{
					logger.LogWarning(ex, $"Errore durante l'aggiornamento in {nameof(PutAuthor)} 😭");
					return StatusCode(500, Messages.Error500Message);
				}
			}

			return NoContent();
		}

		/// <summary>
		/// Crea un nuovo autore.
		/// </summary>
		/// <param name="authorDto">I dati dell'autore da creare.</param>
		/// <returns>Risultato della creazione dell'autore.</returns>
		[HttpPost]
		public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
		{
			try
			{
				var author = mapper.Map<Author>(authorDto);

				await _context.Authors.AddAsync(author);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante la creazione in {nameof(PostAuthor)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// Elimina un autore specifico in base all'ID.
		/// </summary>
		/// <param name="id">L'ID dell'autore da eliminare.</param>
		/// <returns>Risultato dell'operazione di eliminazione.</returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAuthor(int id)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);
				if (author == null)
				{
					logger.LogWarning($"{nameof(Author)} Record non trovato: {nameof(DeleteAuthor)} - ID {id} 😭");
					return NotFound();
				}

				_context.Authors.Remove(author);
				await _context.SaveChangesAsync();

				return NoContent();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante l'eliminazione in {nameof(DeleteAuthor)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// Verifica se un autore esiste in base all'ID.
		/// </summary>
		/// <param name="id">L'ID dell'autore da verificare.</param>
		/// <returns>True se l'autore esiste, altrimenti false.</returns>
		private async Task<bool> AuthorExists(int id)
		{
			return await _context.Authors.AnyAsync(e => e.Id == id);
		}
	}
}
