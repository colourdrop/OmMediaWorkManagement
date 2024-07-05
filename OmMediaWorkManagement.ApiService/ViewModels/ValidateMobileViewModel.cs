using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.ViewModels
{
	public class ValidateMobileViewModel
	{
		[Phone]
		[Required(ErrorMessage = "Please Enter Mobile Number")]
		public string MobileNumber { get; set; }
		public string? Otp { get; set; }
	}
}
