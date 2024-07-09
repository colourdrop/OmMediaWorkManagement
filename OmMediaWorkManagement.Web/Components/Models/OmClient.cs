using System;
using System.ComponentModel.DataAnnotations;

public class OmClient
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Display(Name = "Company Name")]
    public string CompanyName { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Mobile number must start with 6, 7, 8, or 9 and be 10 digits long.")]
    public string MobileNumber { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    
    public string Email { get; set; }

    [Required(ErrorMessage = "Creation date is required")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string? UserId { get; set; }
    public string? UserName { get; set; }
}
