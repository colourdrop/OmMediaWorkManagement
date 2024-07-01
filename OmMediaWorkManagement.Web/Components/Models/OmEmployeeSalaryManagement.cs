namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmEmployeeSalaryManagement
    {
        public int EmployeeSalaryId { get; set; }
        public int OmEmployeeId { get; set; } // Foreign key to OmEmployee


        public decimal? AdvancePayment { get; set; } // Nullable to indicate it's not always applicable
        public DateTime? AdvancePaymentDate { get; set; }
        public decimal? OverTimeSalary { get; set; }
        public decimal? DueBalance { get; set; } // Nullable to indicate it's not always applicable
        public decimal? OverBalance { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
