﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class UserRegistration : IdentityUser
    {
        public string FirstName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpireTime { get; set; }
        public int? Attempts { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual List<OmClient> OmClient { get; set; } = new List<OmClient>();
        public virtual List<OmClientWork> OmClientWork { get; set; } = new List<OmClientWork>();
        public virtual List<OmEmployee> OmEmployee { get; set; } = new List<OmEmployee>();
    }
}
