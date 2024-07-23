using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace OmMediaWorkManagement.Web.Helper
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken) || IsTokenExpired(savedToken))
            {
                await _localStorage.RemoveItemAsync("authToken");
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(string email)
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
            var authState = new AuthenticationState(authenticatedUser);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
            await _localStorage.SetItemAsync("authToken", savedToken); // Set token in local storage if needed
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = new AuthenticationState(anonymousUser);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null)
                return claims;

            if (keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles))
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private bool IsTokenExpired(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length != 3)
                {
                    Console.WriteLine("Invalid JWT format.");
                    return true;
                }

                var payload = parts[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                if (keyValuePairs != null && keyValuePairs.TryGetValue("exp", out object exp))
                {
                    var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.ToString()!));
                    return expTime < DateTimeOffset.UtcNow;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in IsTokenExpired: {ex.Message}");
                return true;
            }
        }

    }
}
