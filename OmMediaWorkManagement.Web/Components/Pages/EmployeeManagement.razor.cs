using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
using Radzen;
using Radzen.Blazor;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class EmployeeManagement
    {
        [Inject]
        public IOmService _OmService { get; set; }
        List<OmEmployee> employees = new List<OmEmployee>();
        IList<OmEmployee> SelectedEmp  ;
        List<OmEmployeeSalaryManagement> salaryManagementData = new List<OmEmployeeSalaryManagement>();
        private string responseMessage = "";
        private Radzen.AlertStyle alertColor = Radzen.AlertStyle.Info;
        private bool showAlert = false;
        private RadzenDataGrid<OmEmployee> employeeGrid = new RadzenDataGrid<OmEmployee>();
        private RadzenDataGrid<OmEmployeeSalaryManagement> salaryManagementGrid = new RadzenDataGrid<OmEmployeeSalaryManagement>();
        OmEmployee selectedEmployee;
        int selectedTabIndex = 0;

        private string columnEditing;
        private List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();
        protected override async Task OnInitializedAsync()
        {
            employees = await _OmService.GetOmEmployees();
            await base.OnInitializedAsync();
            SelectedEmp = new List<OmEmployee>() { employees.FirstOrDefault() };

        }
        private int GetRowIndex(OmEmployee omEmployee)
        {
            return employees.IndexOf(omEmployee);
        } 
        async void OnEmployeeRowClick(DataGridRowMouseEventArgs< OmEmployee> omEmployee)
        {
            if (omEmployee.Data != null)
            {
                salaryManagementData=await _OmService.GetSalaryManagementByEmployeeId(omEmployee.Data.Id);
            }
            salaryManagementGrid.RefreshDataAsync();
        }

        async Task FetchSalaryManagement(int employeeId)
        {
            var salaryManagement=await _OmService.GetSalaryManagementByEmployeeId(employeeId);
        }
        async Task AddSalary()
        {
            // Implement functionality to add salary
            // Example: POST request to add salary
            // HttpClient httpClient = new HttpClient();
            // var response = await httpClient.PostAsync(apiUrl, new StringContent(data, Encoding.UTF8, "application/json"));
            // Handle the response as needed
        }
    }
}
