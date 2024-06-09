using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmClientWork
    {
        public int ClientId { get; set; }

        public DateTime WorkDate { get; set; }

        public string? WorkDetails { get; set; }

        public int PrintCount { get; set; }
        public int Price { get; set; }

        public int Total { get; set; }
        public string? Remarks { get; set; }

    }
}
