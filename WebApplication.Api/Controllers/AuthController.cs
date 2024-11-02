using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationApi.Dati;
using WebApplicationApi.Models.User;
using WebApplicationApi.Staic;

namespace WebApplicationApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous] // Consente l'accesso non autenticato al controller
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> logger;
		private readonly IMapper mapper;
		private readonly UserManager<ApiUser> userManager;
		private readonly IConfiguration configuration;

		// Costruttore del controller per iniettare logger, mappatore, gestore utenti e configurazione dell'app
		public AuthController(ILogger<AuthController> _logger, IMapper _mapper, UserManager<ApiUser> _userManager, IConfiguration _configuration)
		{
			logger = _logger;
			mapper = _mapper;
			userManager = _userManager;
			configuration = _configuration;
		}

		/// <summary>
		/// Metodo per registrare un nuovo utente.
		/// </summary>
		/// <param name="userDto">Oggetto DTO contenente i dati dell'utente (email e password).</param>
		/// <returns>Ritorna un risultato HTTP Accepted se la registrazione va a buon fine o BadRequest se ci sono errori.</returns>
		[HttpPost("Register")]
		public async Task<IActionResult> Register(UserDto userDto)
		{
			// Log dell'evento di tentativo di registrazione
			logger.LogInformation($"Tentativo di registrazione per {userDto.Email}");

			try
			{
				// Mappa il DTO ricevuto al modello ApiUser che verrà salvato nel database
				var utente = mapper.Map<ApiUser>(userDto);
				utente.UserName = userDto.Email;
				// Crea l'utente con la password fornita
				var result = await userManager.CreateAsync(utente, userDto.Password);

				// Se la creazione non è riuscita, aggiungi gli errori al ModelState e ritorna un BadRequest
				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}
					return BadRequest(ModelState);
				}

				// Assegna automaticamente il ruolo "User" al nuovo utente
				await userManager.AddToRoleAsync(utente, "User");
				return Accepted(); // Conferma che l'utente è stato registrato con successo
			}
			catch (Exception ex)
			{
				// Log di errore in caso di eccezione durante la registrazione
				logger.LogError(ex, $"Qualcosa è andato storto in {nameof(Register)} 😭");
				return Problem($"Qualcosa è andato storto in {nameof(Register)}", statusCode: 500);
			}
		}

		/// <summary>
		/// Metodo per effettuare il login di un utente registrato.
		/// </summary>
		/// <param name="userDto">Oggetto DTO contenente email e password dell'utente.</param>
		/// <returns>Ritorna un token JWT se il login ha successo, o Unauthorized se fallisce.</returns>
		[HttpPost("login")]
		public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
		{
			// Log dell'evento di tentativo di login
			logger.LogInformation($"Tentativo di login per {userDto.Email}");

			try
			{
				// Trova l'utente nel database in base all'email
				var user = await userManager.FindByEmailAsync(userDto.Email);
				// Verifica se l'utente esiste e la password è corretta
				if (user == null || !await userManager.CheckPasswordAsync(user, userDto.Password))
				{
					return Unauthorized(); // Se non valido, ritorna 401 Unauthorized
				}

				// Genera un token JWT per l'utente
				string tokenString = await GenerateToken(user);

				// Crea una risposta contenente l'email, l'ID e il token dell'utente
				var response = new AuthResponse
				{
					Email = userDto.Email,
					Token = tokenString,
					UserId = user.Id,
				};

				return Ok(response); // Ritorna il token come risposta HTTP 200 OK
			}
			catch (Exception ex)
			{
				// Log di errore in caso di eccezione durante il login
				logger.LogError(ex, $"Qualcosa è andato storto in {nameof(Login)} 😭");
				return Problem($"Qualcosa è andato storto in {nameof(Login)}", statusCode: 500);
			}
		}

		/// <summary>
		/// Metodo per generare un token JWT per un utente.
		/// </summary>
		/// <param name="user">Oggetto ApiUser per il quale generare il token.</param>
		/// <returns>Ritorna una stringa contenente il token JWT.</returns>
		private async Task<string> GenerateToken(ApiUser user)
		{
			// Genera la chiave di sicurezza dal segreto configurato
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			// Ottiene i ruoli dell'utente e li converte in claim
			var roles = await userManager.GetRolesAsync(user);
			var userClaims = await userManager.GetClaimsAsync(user);
			var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

			// Definisce la lista di claim di base per l'utente
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Soggetto del token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID univoco del token
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // Email dell'utente
                new Claim(CustomClaimTypes.Uid, user.Id) // ID utente personalizzato
            }
			.Union(userClaims) // Aggiunge altri claim specifici dell'utente
			.Union(roleClaims); // Aggiunge i ruoli dell'utente come claim

			// Genera il token con le impostazioni specificate
			var token = new JwtSecurityToken(
				issuer: configuration["JwtSettings:Issuer"], // Emittente del token
				audience: configuration["JwtSettings:Audience"], // Destinatario previsto del token
				claims: claims, // Claim del token
				expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])), // Scadenza del token
				signingCredentials: credentials // Credenziali di firma
			);

			// Ritorna il token JWT come stringa
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}