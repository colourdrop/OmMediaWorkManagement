using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
using Radzen;
using Radzen.Blazor;
using System.ComponentModel.DataAnnotations;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class EmployeeManagement
    {
        [Inject]
        public IOmService _OmService { get; set; }
        List<OmEmployee> employees = new List<OmEmployee>();
        IList<OmEmployee> SelectedEmp;
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
        int progress;
        string info;
        private IBrowserFile selectedFile;
        private List<IBrowserFile> selectedDocumentFiles = new List<IBrowserFile>();
     
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
        async void OnEmployeeRowClick(DataGridRowMouseEventArgs<OmEmployee> omEmployee)
        {
            if (omEmployee.Data != null)
            {
                salaryManagementData = await _OmService.GetSalaryManagementByEmployeeId(omEmployee.Data.Id);
            }
            salaryManagementGrid.RefreshDataAsync();
        }

        async Task FetchSalaryManagement(int employeeId)
        {
            var salaryManagement = await _OmService.GetSalaryManagementByEmployeeId(employeeId);
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
            if (editMode == DataGridEditMode.Single)
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
        private async Task InsertRow()
        {

            var emp = new OmEmployee { CreatedDate = DateTime.UtcNow };
            empToInsert.Add(emp);
            employees.Insert(0, emp);
            await empGrid.EditRow(emp);
            await empGrid.Reload();
            StateHasChanged();
        }
        private void HandleFileSelection(InputFileChangeEventArgs args)
        {
            var file = args.File;
            if (file.Size <= 10485760) // 10 MB
            {
                selectedFile = file;
            }
            else
            {
                Console.WriteLine($"File size exceeds the limit: {file.Size} bytes");
            }
        }

        private void HandleDocumentFileSelection(InputFileChangeEventArgs args)
        {
            foreach (var file in args.GetMultipleFiles())
            {
                if (file.Size <= 10485760) // 10 MB
                {
                    selectedDocumentFiles.Add(file);
                }
                else
                {
                    Console.WriteLine($"File size exceeds the limit: {file.Size} bytes");
                }
            }
        }

        private async Task SaveRow(OmEmployee omEmployee)
        {
            try
            {
                if (selectedFile == null)
                {
                    Console.WriteLine("No file selected.");
                    return;
                }

                AddOmEmployee addOmEmployee = new AddOmEmployee
                {
                    Name = omEmployee.Name,
                    Address = omEmployee.Address,
                    CompanyName = omEmployee.CompanyName,
                    Email = omEmployee.Email,
                    PhoneNumber = omEmployee.PhoneNumber,
                    SalaryAmount = omEmployee.SalaryAmount,
                    IsSalaryPaid = omEmployee.IsSalaryPaid,
                    Description = omEmployee.Description,
                    EmployeeProfile = selectedFile,
                    EmployeeDocuments = selectedDocumentFiles?.ToList()
                };

                
                // Send HTTP request to API endpoint to add employee
                var response = await _OmService.AddEmployee(addOmEmployee);
                if (response.IsSuccessStatusCode)
                {
                    
                    selectedFile = null;
                    selectedDocumentFiles.Clear();
                }
                else
                {
                    Console.WriteLine($"Failed to add employee: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving employee: {ex.Message}");
            }
        }

        private void CancelEdit(OmEmployee omEmployee)
        {
            Reset();
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

        private async Task<IFormFile> CreateFormFileFromIFormFile(IBrowserFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxAllowedSize: 10485760).CopyToAsync(memoryStream); // 10 MB limit
            memoryStream.Position = 0;
            return new FormFile(memoryStream, 0, memoryStream.Length, file.Name, file.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = file.ContentType
            };
        }
    }
}
