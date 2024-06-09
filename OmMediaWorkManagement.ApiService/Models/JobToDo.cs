using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class JobToDo
    {
        [Key]
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public double? Quantity { get; set; }
        public DateTime JobPostedDateTime { get; set; }
        public int? PostedBy { get; set; }
        public int JobStatusType { get; set; }
        public bool IsStatus { get; set; }

        public virtual List<JobImages> JobImages { get; set; } = new List<JobImages>();


    }
    public class JobImages
    {
        [Key]
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public int JobTodoId
        {
            get;
            set;
        }
        [ForeignKey("JobTodoId")]
        public virtual JobToDo JobToDo
        {
            get;
            set;
        }
    }

    public class JobTypeStatus
    {
        [Key]
        public int Id { get; set; }
        public int JobStatusType { get; set; }
        public string JobStatusName
        {
            get; set;
        }
    }
}
