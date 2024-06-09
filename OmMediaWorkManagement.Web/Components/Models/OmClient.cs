using System;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmClient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? MobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Creation date is required")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
