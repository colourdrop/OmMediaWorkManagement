using Blazored.LocalStorage;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components.Authorization;
using OmMediaWorkManagement.Web.AuthInterface;
using OmMediaWorkManagement.Web.AuthModels;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace OmMediaWorkManagement.Web.AuthService
{
    public class AuthenticationService : IAuthenticationService
    {
       
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient httpClient;
        public AuthenticationService(  AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage , HttpClient httpClient)
        {
          
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Login(LoginViewModel userForAuthentication)
        {
            try
            {
                // Create the request URI
                var requestUri = "/api/OmMediaAuth/login";

                // Log the full URL
                var fullUrl = httpClient.BaseAddress + requestUri;
                Console.WriteLine($"Request URL: {fullUrl}");

                // Log the request content
                var requestContent = JsonSerializer.Serialize(userForAuthentication);
                Console.WriteLine($"Request Content: {requestContent}");

                // Send the request
                var response = await httpClient.PostAsJsonAsync(requestUri, userForAuthentication);

                // Check if the request was successful
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> RegisterUser(UserRegistrationViewModel userForRegistration)
        {
            string url = $"/api/OmMediaAuth/registration";

            // Make HTTP PUT request
            var response = await httpClient.PostAsJsonAsync<Object>(url, userForRegistration);

            // Check if the request was successful
            return response;
        }
    }
}
