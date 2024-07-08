using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using OmMediaWorkManagement.Web.AuthInterface;
using OmMediaWorkManagement.Web.AuthModels;
using OmMediaWorkManagement.Web.Helper;
using System.Text.Json;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class Login
    {
        public UserRegistrationViewModel userRegistrationViewModel = new UserRegistrationViewModel();
        public LoginViewModel userForAuthentication = new LoginViewModel();
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        

    [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string responseMessage { get; set; }
        string LastSubmitResult;
        public async Task HandleValidSubmit()
        {
            ShowAuthError = false;
            var response = await AuthenticationService.Login(userForAuthentication);
            response.EnsureSuccessStatusCode();
            responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Parse the JSON response to get the token and expiration
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseMessage);
                var token = authResponse.token;

                await LocalStorage.SetItemAsStringAsync("authToken", token);
                
                // Navigate to the home page or any desired location
                NavigationManager.NavigateTo("/Home", true); // Navigate and reload the page
            }
            else
            {
                ShowAuthError = true;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
