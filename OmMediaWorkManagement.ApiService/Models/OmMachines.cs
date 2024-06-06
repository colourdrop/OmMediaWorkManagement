using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class OmMachines
    {
        [Key]
        public int Id { get; set; } 
        public string? MachineName { get; set; }
        public string? MachineDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRunning { get; set; }
    }
}
