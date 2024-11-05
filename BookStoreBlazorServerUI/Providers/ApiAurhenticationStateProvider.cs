using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreBlazorServerUI.Providers
{
    public class ApiAurhenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localService;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
        public ApiAurhenticationStateProvider(ILocalStorageService localService)
        {
            this.localService = localService;
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Ottiene lo stato di autenticazione attuale dell'utente.
        /// </summary>
        /// <returns>Stato di autenticazione dell'utente.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity()); // Utente non autenticato per impostazione predefinita.
            var saveToken = await localService.GetItemAsync<string>("accessToken"); // Recupera il token JWT dallo storage locale.

            if (saveToken == null)
                return new AuthenticationState(user); // Se non c'è un token, restituisce uno stato di non autenticazione.

            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(saveToken); // Legge e decodifica il token JWT.

            if (tokenContent.ValidFrom < DateTime.Now)
                return new AuthenticationState(user); // Se il token è scaduto, restituisce uno stato di non autenticazione.

            var claims = await GetClaims(); // Ottiene i claim dal token.

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")); // Crea un utente autenticato con i claim dal token.

            return new AuthenticationState(user); // Restituisce lo stato di autenticazione.
        }

        /// <summary>
        /// Metodo che imposta lo stato su autenticato.
        /// </summary>
        public async Task LoggedIn()
        {
            var claims = await GetClaims(); // Ottiene i claim.
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")); // Imposta un utente autenticato.
            var authStarte = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authStarte); // Notifica lo stato di autenticazione cambiato.
        }

        /// <summary>
        /// Metodo che imposta lo stato su non autenticato.
        /// </summary>
        public async Task LoggedOut()
        {
            await localService.RemoveItemAsync("accessToken"); // Rimuove il token dallo storage locale.
            var noBody = new ClaimsPrincipal(new ClaimsIdentity()); // Crea un utente non autenticato.
            var authSate = Task.FromResult(new AuthenticationState(noBody));
            NotifyAuthenticationStateChanged(authSate); // Notifica lo stato di autenticazione cambiato.
        }

        /// <summary>
        /// Ottiene i claim dal token JWT salvato.
        /// </summary>
        /// <returns>Lista di claim contenuti nel token JWT.</returns>
        private async Task<List<Claim>> GetClaims()
        {
            var saveToken = await localService.GetItemAsync<string>("accessToken"); // Recupera il token JWT dallo storage locale.
            var tokenContent = jwtSecurityTokenHandler?.ReadJwtToken(saveToken); // Decodifica il token JWT.
            var claims = tokenContent.Claims.ToList(); // Converte i claim del token in una lista.
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject)); // Aggiunge il nome utente come claim.
            return claims; // Restituisce i claim.
        }
    }
}
