using Blazored.LocalStorage;
using BookStoreBlazorServerUI.Providers;
using BookStoreBlazorServerUI.Serveces.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreBlazorServerUI.Sevices.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IClient httpClient;
        private readonly ILocalStorageService _localService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        /// <summary>
        /// Costruttore che inizializza i servizi HTTP, di storage locale e di stato di autenticazione.
        /// </summary>
        /// <param name="HttpClient">Servizio HTTP per comunicare con l'API.</param>
        /// <param name="localService">Servizio di storage locale per salvare il token JWT.</param>
        /// <param name="authenticationStateProvider">Provider dello stato di autenticazione dell'app.</param>
        public AuthenticationService(IClient HttpClient, ILocalStorageService localService, AuthenticationStateProvider authenticationStateProvider)
        {
            httpClient = HttpClient;
            _localService = localService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Effettua l'autenticazione dell'utente inviando le credenziali al server, salva il token JWT e aggiorna lo stato di autenticazione dell'app.
        /// </summary>
        /// <param name="loginModel">Modello contenente le credenziali di login (email e password).</param>
        /// <returns>Restituisce true se l'autenticazione ha avuto successo.</returns>
        public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
        {
            // Esegue la chiamata HTTP per il login utilizzando le credenziali fornite dall'utente.
            var response = await httpClient.LoginAsync(loginModel);

            // Salva il token JWT nella memoria locale.
            await _localService.SetItemAsync("accessToken", response.Token);

            // Aggiorna lo stato di autenticazione dell'app, impostando l'utente come autenticato.
            await ((ApiAurhenticationStateProvider)authenticationStateProvider).LoggedIn();

            return true; // Ritorna true per indicare il successo dell'autenticazione.
        }


        /// <summary>
        /// Effettua il logout dell'utente, rimuovendo il token e aggiornando lo stato di autenticazione.
        /// </summary>
        public async Task Logout()
        {
            // Aggiorna lo stato di autenticazione dell'app impostando l'utente come non autenticato.
            await ((ApiAurhenticationStateProvider)authenticationStateProvider).LoggedOut();
        }
    }
}
