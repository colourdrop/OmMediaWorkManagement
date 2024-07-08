using System.ComponentModel.DataAnnotations;
namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="UserName is required")]
		public string UserName { get; set; }


		[EmailAddress]
		[Required(ErrorMessage = "EmailAddress   is required")]
		public string EmailAddress { get; set; }        

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Mobile Number is required")]
        public string PhoneNumber { get; set; }
		 
		public string? RoleId
		{
			get;
			set;
		}

	}
}
