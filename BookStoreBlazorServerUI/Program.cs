using Blazored.LocalStorage;
using BookStoreBlazorServerUI.Providers;
using BookStoreBlazorServerUI.Serveces.Base;
using BookStoreBlazorServerUI.Sevices.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region Aggiunge i servizi di Razor Pages e Blazor Server, necessari per eseguire l'applicazione.
builder.Services.AddRazorPages(); // Abilita il supporto per le Razor Pages.
builder.Services.AddServerSideBlazor(); // Abilita Blazor Server per la gestione lato server dei componenti Blazor.
builder.Services.AddBlazoredLocalStorage(); // Aggiunge il servizio Blazored.LocalStorage per gestire lo stato locale (dati salvati nel browser).
#endregion

#region HttpClient
builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://localhost:7297"));
#endregion

#region Configurazione dei servizi di autenticazione

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); // Aggiunge un servizio di autenticazione tramite l’interfaccia e implementazione definite (es. login e registrazione).
builder.Services.AddScoped<ApiAurhenticationStateProvider>(); // Aggiunge il provider personalizzato per lo stato dell’autenticazione.
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
                p.GetRequiredService<ApiAurhenticationStateProvider>());
// Registra AuthenticationStateProvider e lo associa al provider personalizzato, rendendo l'autenticazione monitorabile dall'app.

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection(); // Reindirizza tutte le richieste HTTP a HTTPS per maggiore sicurezza.

app.UseStaticFiles(); // Permette di servire file statici come CSS, immagini e JavaScript.

app.UseRouting(); // Abilita l'instradamento per gestire le richieste in base agli endpoint definiti.

app.MapBlazorHub(); // Configura il SignalR Hub di Blazor, permettendo la comunicazione bidirezionale tra client e server per componenti Blazor.

app.MapFallbackToPage("/_Host"); // Imposta una pagina di fallback (_Host.cshtml) che gestisce le richieste non mappate ad altri endpoint.

app.Run();
