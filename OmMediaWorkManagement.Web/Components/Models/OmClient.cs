using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmClient
    { 
        public int Id { get; set; }        
        public string Name { get; set; }
        public string? CompanyName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
