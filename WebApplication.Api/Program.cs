using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configurazione Serilog e CORS
// Configurazione di Serilog come sistema di logging
builder.Host.UseSerilog((ctx, lc) =>
	lc.WriteTo.Console()
	.ReadFrom.Configuration(ctx.Configuration)
);

// Configura CORS (Cross-Origin Resource Sharing) per consentire richieste da qualsiasi origine.
// Questa politica permette qualsiasi metodo, intestazione e origine nelle richieste cross-origin,
// rendendo l'API accessibile da qualsiasi dominio.

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", b => b.AllowAnyMethod()      // Consente qualsiasi metodo HTTP (GET, POST, PUT, DELETE, ecc.)
										.AllowAnyHeader()      // Consente qualsiasi intestazione HTTP (incluso Authorization, Content-Type, ecc.)
										.AllowAnyOrigin());    // Consente qualsiasi origine (dominio) di effettuare richieste all'API
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// chiedo di usarlo
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
