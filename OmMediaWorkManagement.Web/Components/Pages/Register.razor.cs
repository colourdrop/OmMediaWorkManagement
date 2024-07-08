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
            
            UserRegistration userRegistration = new UserRegistration()
            {
                FirstName = userRegistrationViewModel.FirstName,
                UserName = userRegistrationViewModel.UserName,
                EmailAddress = userRegistrationViewModel.EmailAddress,
                Password = userRegistrationViewModel.Password, // Use Password instead of ConfirmPassword
                PhoneNumber = userRegistrationViewModel.PhoneNumber,
                RoleId = "", // Ensure RoleId is set as needed
             
            };

            ShowAuthError = false;

            try
            {
                var response = await AuthenticationService.RegisterUser(userRegistration);

                if (response.IsSuccessStatusCode)
                {
                    ShowAuthError = true;
                    // Optionally handle success behavior here
                }
                else
                {
                    ShowAuthError = false;
                    // Handle error or navigate away as needed
                    NavigationManager.NavigateTo("/");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                ShowAuthError = false;
                NavigationManager.NavigateTo("/"); // Example fallback navigation
            }
        }

    }
}
