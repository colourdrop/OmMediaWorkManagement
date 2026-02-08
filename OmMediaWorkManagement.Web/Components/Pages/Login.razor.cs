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
        private Radzen.AlertStyle alertColor = Radzen.AlertStyle.Info;
        public string responseMessage { get; set; }
        string LastSubmitResult;
        private bool showAlert = false;
        private bool isLoading = false;

        public async Task HandleValidSubmit()
        {
            ShowAuthError = false;
            isLoading = true;
            var response = await AuthenticationService.Login(userForAuthentication);
      
            responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                responseMessage = "Welcome to Om Media Solution";
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true;

                await Task.Delay(800);
                NavigationManager.NavigateTo("/OmHome", true);
            }

            else
            {
                responseMessage = "User Not Exist or Bad Credentials";
                alertColor = Radzen.AlertStyle.Danger;
                showAlert = true;
              
            }
            isLoading = false;
        }



    }
}
