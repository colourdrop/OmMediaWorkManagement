using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.AuthInterface;
using OmMediaWorkManagement.Web.AuthModels;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class Register
    {
        public UserRegistrationViewModel userRegistrationViewModel = new UserRegistrationViewModel();
        
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
            var response = await AuthenticationService.RegisterUser(userRegistrationViewModel);
            response.EnsureSuccessStatusCode();
            responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == true)
            {


                ShowAuthError = true;
            }
            else
            {
                ShowAuthError = false;
                NavigationManager.NavigateTo("/");
            }
        }
        
    }
}
