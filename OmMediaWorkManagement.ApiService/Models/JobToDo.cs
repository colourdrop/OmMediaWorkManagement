using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class JobToDo
    {
        [Key]
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Quantity { get; set; }
        public byte[]? Image { get; set; }
        public DateTime JobPostedDateTime { get; set; }
        public int? PostedBy { get; set; }
        
        public bool? JobIsRunning { get; set; }
        public bool? JobIsFinished { get; set; }
        public bool? JobIsHold { get; set; }
        public bool? JobIsDeclained { get; set; }
        

    }
}
