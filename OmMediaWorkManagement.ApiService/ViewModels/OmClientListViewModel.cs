using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class OmClientListViewModel
    {
        
        public int Id { get; set; }
     
        public string Name { get; set; }
        public string? CompanyName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
