using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class UserRegistration : IdentityUser
    {
        public Int64 UserRegistrationId { get; set; }
        public string UserName {  get; set; }
        public string Password { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpireTime { get; set; }
        public int? Attempts { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
