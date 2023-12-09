using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blog.Common.Constant;
using Blog.Server.Helper;

namespace Blog.Server.Service
{
    public class AuthProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public ClaimsPrincipal User { get; private set; }

        public AuthProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsStringAsync(Constant.Token);

            if (string.IsNullOrWhiteSpace(token))
            {
                User = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(User);
            }

            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            User = new ClaimsPrincipal(identity);

            return new AuthenticationState(User);
        }

        public void NotifyUserLoggedIn(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            User = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(User));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
