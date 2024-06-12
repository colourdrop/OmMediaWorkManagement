using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class JobToDo
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        
        public double? Quantity { get; set; }
        public bool? IsStatus { get; set; }
        public int JobStatusType { get; set; }
        public string JobStatusName { get; set; }

        public DateTime? JobPostedDateTime { get; set; }
        public List<string>? Images { get; set; }
    }
}
