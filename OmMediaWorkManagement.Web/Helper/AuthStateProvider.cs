using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace OmMediaWorkManagement.Web.Helper
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationState _anonymous;
        private bool _isInitialized;
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _isInitialized = false;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Wait until the component is fully rendered
            if (!_isInitialized)
            {
                return _anonymous;
            }

            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public async Task InitializeAsync()
        {
            _isInitialized = true;
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public void NotifyUserAuthentication(string email, string token)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use this if serving over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1) // Set cookie expiration as needed
            });

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("authToken");

            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
