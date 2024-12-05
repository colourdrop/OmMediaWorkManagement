using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmMediaWorkManagement.ApiService.Models
{
	public class JobToDo
	{
		[Key]
		public int Id { get; set; }
		public int OmClientId
		{
			get;
			set;
		}
		[ForeignKey("OmClientId")]
		public virtual OmClient OmClient
		{
			get;
			set;
		}
		public string? CompanyName { get; set; }
		public double? Quantity { get; set; }
		public decimal? Price { get; set; }
        public int? PaidAmount { get; set; }
        public int? TotalPayable { get; set; }
        public int? DueBalance { get; set; }
        public int? total { get; set; }
		public string? Description { get; set; }
		public DateTime JobPostedDateTime { get; set; }
		public int? PostedBy { get; set; }
		public int JobStatusType { get; set; }
		public bool IsStatus { get; set; }
        public int OmEmpId { get; set; }

        [ForeignKey("OmEmpId")]
        public virtual OmEmployee OmEmployee
        {
            get;
            set;
        }
        public virtual List<JobImages> JobImages { get; set; } = new List<JobImages>();


	}
	public class JobImages
	{
		[Key]
		public int Id { get; set; }
		public string ImagePath { get; set; }
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
