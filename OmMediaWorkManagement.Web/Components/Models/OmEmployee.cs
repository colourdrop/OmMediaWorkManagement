using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.Web.Components.Models
{
    public class OmEmployee
    {
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
        public List<string> EmployeeDocuments {  get; set; }
    }
    public class AddOmEmployee
    {
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
        public IFormFile? EmployeeProfile { get; set; }
        public List<IFormFile>? EmployeeDocuments { get; set; }

        public bool IsDeleted { get; set; }
    }
}
