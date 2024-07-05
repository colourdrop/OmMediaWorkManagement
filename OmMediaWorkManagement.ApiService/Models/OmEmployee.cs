using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.Models
{
    public class OmEmployee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }      
        public string Address { get; set; }
        public string? CompanyName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public decimal SalaryAmount { get; set; }
        public bool IsSalaryPaid { get; set; }
        public string? Description { get; set; }    
        public string? EmployeeProfilePath { get; set; }
        public DateTime CreatedDate { get; set; }   
        public bool IsDeleted { get; set; }
		public int AppPin { get; set; }
		public DateTime? OTPGeneratedTime { get; set; }
		public string? OTP { get; set; }
		public DateTime? OTPExpireTime { get; set; }
		public int OTPAttempts { get; set; }
		public ICollection<OmEmployeeDocuments> EmployeeDocuments { get; set; }
    }

    public class OmEmployeeDocuments
    {
        [Key]
        public int OmEmployeeDocumentId { get; set; }
        public int OmEmployeeId { get; set; } // Foreign key to OmEmployee
        public string? EmployeeDocumentsPath { get; set; }

        // Navigation property
        public OmEmployee OmEmployee { get; set; }
    }

    public class OmEmployeeSalaryManagement
    {
        [Key]
        public int EmployeeSalaryId { get; set; }
        public int OmEmployeeId { get; set; } // Foreign key to OmEmployee


        public decimal? AdvancePayment { get; set; } // Nullable to indicate it's not always applicable
        public DateTime? AdvancePaymentDate { get; set; }
        public decimal? OverTimeSalary { get; set; }
        public decimal? DueBalance { get; set; } // Nullable to indicate it's not always applicable
        public decimal? OverBalance { get; set; }
        public DateTime? CreatedDate { get; set; }
        // Navigation property
        public OmEmployee OmEmployee { get; set; }
    }

    public class OmEmployeeShift
    {
        [Key]
        public int EmployeeShiftId { get; set; }
        public int OmEmployeeId { get; set; } // Foreign key to OmEmployee
        public string ShiftTiming { get; set; }
        public string? ShiftOverTime { get; set; } // Nullable to indicate it's not always applicable
        public decimal OverTimePerHourCost { get; set; } 

        // Navigation property
        public OmEmployee OmEmployee { get; set; }
    }
}
