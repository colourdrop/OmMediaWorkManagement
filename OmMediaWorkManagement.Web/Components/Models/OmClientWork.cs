using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmClientWork
    {
        public int Id { get; set; }

        public DateTime WorkDate { get; set; }

        public string? WorkDetails { get; set; }

        public int PrintCount { get; set; }
        public int Price { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsSMSSent { get; set; }
        public bool IsPushSent { get; set; }
        public int Total { get; set; }
        public string? Remarks { get; set; }
        public int OmClientId { get; set; }

    }
}
