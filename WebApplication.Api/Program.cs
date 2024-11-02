using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using WebApplicationApi.Configurations;
using WebApplicationApi.Dati;

var builder = WebApplication.CreateBuilder(args);

// Aggiunge i servizi al contenitore.

#region String Connection
// Recupera la stringa di connessione al database da "appsettings.json" 
// usando la chiave "BookStoreAppDbConnection" e la salva in conString.
var conString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection");

// Registra il contesto di database BookStoreDbContext per gestire le operazioni sul database 
// utilizzando SQL Server e la stringa di connessione definita sopra.
builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(conString));
#endregion

#region Configurazione dei servizi di Identity
// Configura il sistema di Identity per gestire l'autenticazione e l'autorizzazione 
// tramite ruoli per l'applicazione API, utilizzando ApiUser e IdentityRole.
builder.Services.AddIdentityCore<ApiUser>()
	// Aggiunge il supporto per la gestione dei ruoli con IdentityRole.
	.AddRoles<IdentityRole>()
	// Collega Identity al contesto BookStoreDbContext per salvare e recuperare i dati dal database.
	.AddEntityFrameworkStores<BookStoreDbContext>();
#endregion

#region Configurazione di AutoMapper
// Registra AutoMapper come servizio per mappare oggetti tra tipi diversi,
// utilizzando la classe di configurazione MapperConfig definita nel progetto.
builder.Services.AddAutoMapper(typeof(MapperConfig));
#endregion

// Aggiunge il supporto per i controller al progetto.
builder.Services.AddControllers();

// Configura Swagger/OpenAPI per la generazione della documentazione interattiva dell'API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configurazione di Serilog
// Configura Serilog come sistema di logging leggendo le impostazioni dal file di configurazione
// e stampando i log su console.
builder.Host.UseSerilog((ctx, lc) =>
	lc.WriteTo.Console()
	  .ReadFrom.Configuration(ctx.Configuration)
);
#endregion

#region Configurazione del CORS (Cross-Origin Resource Sharing)
// Configura le politiche di CORS per consentire l'accesso all'API da qualsiasi origine
// con qualsiasi metodo e intestazione, utilizzando una politica chiamata "AllowAll".
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", b => b.AllowAnyMethod()     // Consente tutti i metodi HTTP (GET, POST, ecc.)
										.AllowAnyHeader()     // Consente tutte le intestazioni (Authorization, Content-Type, ecc.)
										.AllowAnyOrigin());   // Consente qualsiasi origine di effettuare richieste.
});
#endregion

// Configura il servizio di autenticazione utilizzando JWT (JSON Web Token).
// Specifica come validare i token e impostare parametri come il Key, l'Audience e l'Issuer.
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,                            // Verifica la firma del token.
		ValidateIssuer = true,                                      // Verifica l'issuer del token.
		ValidateAudience = true,                                    // Verifica l'audience del token.
		ValidateLifetime = true,                                    // Verifica la validità del token.
		ClockSkew = TimeSpan.Zero,                                  // Disabilita lo scarto temporale.
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],  // Imposta l'issuer valido.
		ValidAudience = builder.Configuration["JwtSettings:Audience"], // Imposta l'audience valida.
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:key"])) // Imposta la chiave di firma.
	};
});

var app = builder.Build();


// Configura il middleware per il ciclo di vita delle richieste HTTP.
if (app.Environment.IsDevelopment())
{
	// Abilita Swagger per la documentazione interattiva in ambiente di sviluppo.
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // Forza le richieste HTTP su HTTPS.

app.UseCors("AllowAll");    // Abilita la politica CORS "AllowAll" per tutte le richieste.

app.UseAuthentication();     // Abilita l'autenticazione per il processo di autorizzazione.

app.UseAuthorization();      // Abilita il middleware di autorizzazione per controllare i permessi di accesso.

app.MapControllers();        // Mappa i controller definiti nel progetto agli endpoint corrispondenti.

app.Run();                   // Avvia l'applicazione.
