using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class JobToDoViewModel
    {
      
        public int OmClientId { get; set; }
     

        public double Quantity { get; set; }
        public int? Price { get; set; }
        public int? PaidAmount { get; set; }
        public int? DueBalance { get; set; }
        public int? TotalPayable { get; set; }
        public int? total { get; set; }
        public string? Description { get; set; }

        public List<IFormFile?>? Images { get; set; }

        public bool IsStatus { get; set; }
        public int JobStatusType { get; set; }

    }
}
