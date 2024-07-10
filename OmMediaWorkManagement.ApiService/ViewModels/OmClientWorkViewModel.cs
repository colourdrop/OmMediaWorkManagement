using OmMediaWorkManagement.ApiService.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class OmClientWorkViewModel
    {

        public int ClientId { get; set; }

        public DateTime WorkDate { get; set; }

        public string? WorkDetails { get; set; }

        public int PrintCount { get; set; }
        public int Price { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDeleted { get; set; }
        public int? PaidAmount { get; set; }
        public int? TotalPayable { get; set; }
        public int? DueBalance { get; set; }
        public int? Total { get; set; }
        public string? Remarks { get; set; }


    }
}
