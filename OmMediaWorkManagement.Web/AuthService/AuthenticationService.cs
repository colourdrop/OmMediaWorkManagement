using Blazored.LocalStorage;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components.Authorization;
using OmMediaWorkManagement.Web.AuthInterface;
using OmMediaWorkManagement.Web.AuthModels;
using OmMediaWorkManagement.Web.Helper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OmMediaWorkManagement.Web.AuthService
{
    public class AuthenticationService : IAuthenticationService
    {


        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient httpClient;
        public AuthenticationService(AuthenticationStateProvider authenticationStateProvider ,ILocalStorageService localStorage , HttpClient httpClient)
        {
           
           _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Login(LoginViewModel userForAuthentication)
        {
            try
            {
                // Create the request URI
                var requestUri = "/omapi/api/OmMediaAuth/login";                 
                var fullUrl = httpClient.BaseAddress + requestUri;          
                             

                // Send the request
                var response = await httpClient.PostAsJsonAsync(requestUri, userForAuthentication);
                if (response.IsSuccessStatusCode == false)
                {
                    return response;
                }
                var gettoken = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(gettoken);
                var token = authResponse.token;

                
                await _localStorage.SetItemAsync("authToken", token);
                ((AuthStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(userForAuthentication.Username);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<HttpResponseMessage> RegisterUser(UserRegistration userForRegistration)
        {
            
                
                var response = await httpClient.PostAsJsonAsync("/omapi/api/OmMediaAuth/registration", userForRegistration);


                return response;
             
        }

    }
}
