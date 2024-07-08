using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.AuthModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "EmailAddress is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public string? RoleId { get; set; }
    }

    public class UserRegistration
    {
         
        public string FirstName { get; set; }
      
        public string UserName { get; set; }
 
        public string EmailAddress { get; set; }

      
        public string Password { get; set; }
 
        public string PhoneNumber { get; set; }
    
        public string? RoleId
        {
            get;
            set;
        }

    }
}
