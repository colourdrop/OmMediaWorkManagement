namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmEmployeeSalaryManagement
    {
        public int EmployeeSalaryId { get; set; }
        public int OmEmployeeId { get; set; } // Foreign key to OmEmployees


        public decimal? AdvancePayment { get; set; } // Nullable to indicate it's not always applicable
        public DateTime? AdvancePaymentDate { get; set; }
        public decimal? OverTimeSalary { get; set; }
        public decimal? DueBalance { get; set; } // Nullable to indicate it's not always applicable
        public decimal? OverBalance { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class AddOmEmployeeSalaryManagement
    {
        public int? OmEmployeeId { get; set; }
        public int? salaryManagementid { get; set; }

        public decimal? AdvancePayment { get; set; }

        public decimal? OverTimeSalary { get; set; }
        public decimal? DueBalance { get; set; }
        public decimal? OverBalance { get; set; }

    }
}
