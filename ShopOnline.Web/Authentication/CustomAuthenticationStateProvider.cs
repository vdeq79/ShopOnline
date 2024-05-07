using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Extensions;
using System.Security.Claims;

namespace ShopOnline.Web.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage)
        {
            _sessionStorageService = sessionStorage;        
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorageService.ReadEncryptedItemAsync<UserDto>("UserSession");

                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {   new Claim(ClaimTypes.NameIdentifier, userSession.Id.ToString()),
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Email, userSession.Email),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception)
            {

                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

        }

        public async Task UpdateAuthenticationState(UserDto? userSession)
        {
            if (userSession != null)
            {
                userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
                await _sessionStorageService.SaveItemEncryptedAsync("UserSession", userSession);
            }
            else
            {
                await _sessionStorageService.RemoveItemAsync("UserSession");
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<string> GetToken()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _sessionStorageService.ReadEncryptedItemAsync<UserDto>("UserSession");
                if (userSession != null && DateTime.Now < userSession.ExpiryTimeStamp)
                {
                    result = userSession.Token;
                }
            }
            catch { }

            return result;
        }

    }
}
