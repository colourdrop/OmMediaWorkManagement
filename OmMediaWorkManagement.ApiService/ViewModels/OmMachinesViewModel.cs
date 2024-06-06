namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class OmMachinesViewModel
    {
        
        public string? MachineName { get; set; }
        public string? MachineDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRunning { get; set; }
    }
}
