using System.ComponentModel.DataAnnotations;
namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Mobile Number is required")]
        public string PhoneNumber { get; set; }



    }
}
