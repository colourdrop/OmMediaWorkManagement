using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class OmClient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? CompanyName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<OmClientWork> OmClientWork { get; set; } = new List<OmClientWork>();
    }
}
