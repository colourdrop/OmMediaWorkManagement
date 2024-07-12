using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
using Radzen;
using Radzen.Blazor;
using System.ComponentModel.DataAnnotations;

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
        private RadzenDataGrid<OmEmployee> empGrid;
        private RadzenDataGrid<OmEmployee> employeeGrid = new RadzenDataGrid<OmEmployee>();
        private RadzenDataGrid<OmEmployeeSalaryManagement> salaryManagementGrid = new RadzenDataGrid<OmEmployeeSalaryManagement>();
        OmEmployee selectedEmployee;
        int selectedTabIndex = 0;
        private DataGridEditMode editMode = DataGridEditMode.Multiple;
        private string columnEditing;
        private List<OmEmployee> empToInsert = new List<OmEmployee>();
        private List<OmEmployee> empToUpdate = new List<OmEmployee>();
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
        private void Reset()
        {
            empToInsert.Clear();
            empToUpdate.Clear();
        }
        private async Task EditRow(OmEmployee omEmployee)
        {
            if (editMode == DataGridEditMode.Single  )
            {
                Reset();
            }

            empToUpdate.Add(omEmployee);
            await empGrid.EditRow(omEmployee);
        }
        private void OnUpdateRow(OmEmployee omEmployee)
        {
            Reset();
            //Reset(client);

        }
        private async Task InsertRow( )
        {
            
                var emp = new OmEmployee { CreatedDate = DateTime.UtcNow };
            empToInsert.Add(emp);
            employees.Insert(0, emp);
            await empGrid.EditRow(emp);
            await empGrid.Reload();
            StateHasChanged();
        }
        private async Task SaveRow(OmEmployee omEmployee)
        {
            //CalculateTotal(toDo);
            //var validationResults = new List<ValidationResult>();
            //var isValid = Validator.TryValidateObject(toDo, new ValidationContext(toDo), validationResults, true);
            //if (isValid)
            //{
            //    if (toDo.Id != 0 && toDo.Price != null)
            //    {
            //        JobToDoViewModel jobToDoViewModel = new JobToDoViewModel()
            //        {
            //            OmClientId = toDo.OmClientId,
            //            ClientName = toDo.ClientName,
            //            ComapnyName = toDo.CompanyName,
            //            Quantity = (int)toDo.Quantity,
            //            Price = (int)toDo.Price,
            //            TotalPayable = toDo.TotalPayable,
            //            DueBalance = toDo.DueBalance,
            //            PaidAmount = toDo.PaidAmount,
            //            total = 0,
            //            Description = toDo.Description,
            //            IsStatus = toDo.IsStatus,
            //            JobStatusType = toDo.JobStatusType,
            //        };
            //        var response = await OmService.UpdateJobtToDo(toDo.Id, jobToDoViewModel);
            //        response.EnsureSuccessStatusCode();
            //        responseMessage = await response.Content.ReadAsStringAsync();

            //        if (response.IsSuccessStatusCode == true)
            //        {
            //            alertColor = Radzen.AlertStyle.Success;
            //        }
            //        else
            //        {
            //            alertColor = Radzen.AlertStyle.Danger;

            //        }
            //        showAlert = true; // Show alert
            //    }
            //    else
            //    {
            //        await todoGrid.UpdateRow(toDo);

            //    }
            //}
            //else
            //{
            //    responseMessage = "Please fill all columns";
            //    alertColor = Radzen.AlertStyle.Warning;
            //    showAlert = true; // Show alert
            //}
            //await RefreshTable();
            //await todoGrid.Reload();

        }

        private void CancelEdit(OmEmployee omEmployee)
        {
            Reset( );
            empGrid.CancelEditRow(omEmployee);


        }

        private async Task DeleteRow(OmEmployee omEmployee)
        {
            //Reset(jobToDo);

            //if (todos.Contains(jobToDo))
            //{
            //    var response = await OmService.DeleteJobTodo(jobToDo.Id);
            //    response.EnsureSuccessStatusCode();
            //    responseMessage = await response.Content.ReadAsStringAsync();

            //    if (response.IsSuccessStatusCode == true)
            //    {
            //        alertColor = Radzen.AlertStyle.Success;
            //    }
            //    else
            //    {
            //        alertColor = Radzen.AlertStyle.Danger;

            //    }
            //    showAlert = true; // Show alert
            //    todos = todos.Where(c => c.Id != jobToDo.Id).ToList();
            //    await todoGrid.Reload();
            //}
            //else
            //{
            //    todoGrid.CancelEditRow(jobToDo);
            //    await todoGrid.Reload();
            //}
        }

    }
}
