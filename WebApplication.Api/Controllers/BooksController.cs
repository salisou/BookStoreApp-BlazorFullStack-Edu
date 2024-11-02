using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Dati;
using WebApplicationApi.Models.Book;
using WebApplicationApi.Staic;

namespace WebApplicationApi.Controllers
{
	// Controller che gestisce le operazioni CRUD (Create, Read, Update, Delete) sui libri
	[Route("api/[controller]")]
	[ApiController]
	[Authorize] // Autorizzazione necessaria per accedere al controller.
	public class BooksController : ControllerBase
	{
		private readonly BookStoreDbContext _context;
		private readonly IMapper mapper;
		private readonly ILogger<BooksController> logger;

		// Costruttore del controller: riceve il contesto del database, il mapper per AutoMapper e il logger per il logging delle operazioni
		public BooksController(BookStoreDbContext context, IMapper _mapper, ILogger<BooksController> _logger)
		{
			_context = context;
			this.mapper = _mapper;
			this.logger = _logger;
		}

		/// <summary>
		/// Metodo per ottenere l’elenco di tutti i libri.
		/// GET: api/Books
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
		{
			logger.LogInformation($"Richiesta a {nameof(GetBooks)}👤");

			try
			{
				// Recupera la lista dei libri dal database, inclusi i dati dell'autore
				// Utilizza AutoMapper per proiettare i dati sui DTO (Data Transfer Object)
				var bookDtos = await _context.Books
					.Include(q => q.Author)
					.ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
					.ToListAsync();

				return Ok(bookDtos);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante il recupero in {nameof(GetBooks)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// GET: api/Books/5
		/// Metodo per ottenere i dettagli di un singolo libro in base all’ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<BookDetailsDto>> GetBookBookById(int id)
		{
			try
			{
				// Recupera il libro specifico per ID, incluso l'autore
				var book = await _context.Books
					.Include(q => q.Author)
					.ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
					.FirstOrDefaultAsync(q => q.Id == id);

				// Verrifica se il libro non esiste, restituisce il Record non trovato
				if (book == null)
				{
					logger.LogWarning($"Record non trovato: {nameof(GetBookBookById)} - ID {id}");
					return NotFound();
				}

				return Ok(book);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante il recupero in {nameof(GetBookBookById)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// PUT: api/Books/5
		/// Metodo per aggiornare le informazioni di un libro esistente
		/// </summary>
		/// <param name="id"></param>
		/// <param name="bookDto"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		[Authorize(Roles = "Administrator")]// Permesso di aggiornamento solo per gli amministratori.
		public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
		{
			// Controlla che l'ID corrisponda 
			if (id != bookDto.Id)
			{
				logger.LogWarning($"ID di aggiornamento non valido in: {nameof(PutBook)} - ID {id} 😭");
				return BadRequest(); // Richiesta errata
			}

			// Recupera il libro dal database
			var book = await _context.Books.FindAsync(id);

			if (book == null)
			{
				logger.LogWarning($"{nameof(Author)} Record non trovato: {nameof(PutBook)} - ID {id} 😭");
				return NotFound();
			}

			// Mappa i dati dal DTO al modello libro
			mapper.Map(bookDto, book);
			_context.Entry(book).State = EntityState.Modified;


			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				if (!await BookExists(id))
				{
					logger.LogWarning($"{nameof(Book)} Record non trovato: {nameof(PutBook)} - ID {id} 😭");
					return NotFound();
				}
				else
				{
					logger.LogWarning(ex, $"Errore durante l'aggiornamento in {nameof(PutBook)} 😭");
					return StatusCode(500, Messages.Error500Message);
				}
			}

			return NoContent();
		}

		/// <summary>
		/// POST: api/Books
		/// Metodo per creare un nuovo libro
		/// </summary>
		/// <param name="bookDto"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = "Administrator")]
		public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
		{

			try
			{
				// Mappa i dati dal DTO al modello libro
				var book = mapper.Map<Book>(bookDto);
				_context.Books.Add(book);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetBookBookById), new { id = book.Id }, book);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Errore durante la creazione in {nameof(PostBook)} 😭");
				return StatusCode(500, Messages.Error500Message);
			}
		}

		/// <summary>
		/// DELETE: api/Books/5
		/// Metodo per eliminare un libro esistente
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			// Cerca il libro per ID
			var book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		/// <summary>
		/// // Metodo di supporto per verificare se un libro esiste nel database
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private async Task<bool> BookExists(int id)
		{
			return await _context.Books.AnyAsync(e => e.Id == id);
		}
	}
}
