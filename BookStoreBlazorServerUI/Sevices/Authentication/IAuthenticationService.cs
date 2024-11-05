using BookStoreBlazorServerUI.Serveces.Base;

namespace BookStoreBlazorServerUI.Sevices.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAsync(LoginUserDto loginModel);
        public Task Logout();
    }
}
