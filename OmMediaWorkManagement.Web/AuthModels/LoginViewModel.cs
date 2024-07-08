using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.AuthModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
