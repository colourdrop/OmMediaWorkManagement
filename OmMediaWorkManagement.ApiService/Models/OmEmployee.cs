using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class OmEmployee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
