using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class JobToDo
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }

        public string? CompanyName { get; set; }
        public string? Description { get; set; }        
        public double? Quantity { get; set; }
        public int? Price { get; set; }
        public int? Total { get; set; }

        public bool IsStatus { get; set; }
        public int JobStatusType { get; set; }
        public string JobStatusName { get; set; }

        public DateTime? JobPostedDateTime { get; set; }
        public List<string>? Images { get; set; }
    }
    public class UpdateTodo
    {
        public string? ClientName { get; set; }
        public string? ComapnyName { get; set; }

        public double Quantity { get; set; }
        public int? Price { get; set; }
        public int? total { get; set; }
        public string? Description { get; set; }

        public List<IFormFile?>? Images { get; set; }

        public bool IsStatus { get; set; }
        public int JobStatusType { get; set; }
    }
}
