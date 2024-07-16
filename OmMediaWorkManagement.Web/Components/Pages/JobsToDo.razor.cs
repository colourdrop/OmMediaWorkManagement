using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
using OmMediaWorkManagement.Web.Helper;
using Radzen;
using Radzen.Blazor;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class JobsToDo
    {
        [Inject]
        public IOmService OmService { get; set; }
        private bool showDialog = false;
        string imagePreview;
        private int selectedClientId;
        List<string> imageUrls = new List<string>();
        private bool showAddDialog = false;
        private string responseMessage = "";
        private Radzen.AlertStyle alertColor = Radzen.AlertStyle.Info;
        private bool showAlert = false;

        IEnumerable<JobToDo> jobToDo;
        bool allowRowSelectOnRowClick = false;

        IList<JobToDo> selectedOmClientWork;

        private RadzenDataGrid<JobToDo> todoGrid;
        private List<IFormFile> images = new List<IFormFile>();
        private JobToDoViewModel newJobViewModel = new JobToDoViewModel();
        public List<OmClient> clients { get; set; } = new List<OmClient>();
        public List<JobToDo> todos { get; set; } = new List<JobToDo>();
        private List<JobToDo> filteredtodos = new List<JobToDo>();
        public List<JobTypeStatusViewModel> jobTypeStatusViewModels = new List<JobTypeStatusViewModel>();
        private List<JobToDo> todoToInsert = new List<JobToDo>();
        private List<JobToDo> todoToUpdate = new List<JobToDo>();
        private DataGridEditMode editMode = DataGridEditMode.Multiple;
        private string columnEditing;
        private List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();
        [Inject]
        public IPdfService _pdfService { get; set; }
        private bool IsFirstRender { get; set; } = true;
        private bool IsDelete=false;
        private int GetRowIndex(JobToDo client)
        {
            return todos.IndexOf(client);
        }
        private void Reset()
        {
            todoToInsert.Clear();
            todoToUpdate.Clear();
        }

        private void Reset(JobToDo client)
        {
            todoToInsert.Remove(client);
            todoToUpdate.Remove(client);
        }

        protected override async Task OnInitializedAsync()
        {

            filteredtodos = await OmService.GetJobToDos();
            jobToDo = filteredtodos;
            jobTypeStatusViewModels = await OmService.GetJobTypeStatusList();
            clients = await OmService.GetAllClients();
        }
        private async Task OnClientSelected(object value)
        {
            selectedClientId = (int)value;
            if (selectedClientId != 0)
            {
                filteredtodos = await OmService.GetJobsToDosByClientId(selectedClientId);
            }

            else
            {
                filteredtodos = await OmService.GetJobToDos();
            }
            await todoGrid.Reload(); // Reload the grid to display the new data

        }

        private string GetClientName(int clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId)?.Name ?? "Unknown Client";
        }
        private string GetClientCompanyName(int clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId)?.CompanyName ?? "Unknown Client";
        }
        private async Task EditRow(JobToDo client)
        {
            if (editMode == DataGridEditMode.Single && todoToInsert.Count() > 0)
            {
                Reset();
            }

            todoToUpdate.Add(client);
            await todoGrid.EditRow(client);
        }

        private void OnUpdateRow(JobToDo client)
        {
            Reset(client);

        }

        private async Task SaveRow(JobToDo toDo)
        {
            CalculateTotal(toDo);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(toDo, new ValidationContext(toDo), validationResults, true);
            if (isValid)
            {
                if (toDo.Id != 0&& toDo.Price!= null  )
                {
                    JobToDoViewModel jobToDoViewModel = new JobToDoViewModel()
                    {
                        OmClientId=toDo.OmClientId,
                        ClientName = toDo.ClientName,
                        ComapnyName = toDo.CompanyName,
                        Quantity = (int)toDo.Quantity,
                        Price = (int)toDo.Price,
                        TotalPayable = toDo.TotalPayable,
                        DueBalance = toDo.DueBalance,
                        PaidAmount = toDo.PaidAmount,
                        total = 0,                       
                        Description = toDo.Description,
                        IsStatus = toDo.IsStatus,
                        JobStatusType = toDo.JobStatusType,
                    };
                    var response = await OmService.UpdateJobtToDo(toDo.Id, jobToDoViewModel);
                    response.EnsureSuccessStatusCode();
                    responseMessage = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode == true)
                    {
                        alertColor = Radzen.AlertStyle.Success;
                    }
                    else
                    {
                        alertColor = Radzen.AlertStyle.Danger;

                    }
                    showAlert = true; // Show alert
                }
                else
                {
                    await todoGrid.UpdateRow(toDo);

                }
            }
            else
            {
                responseMessage = "Please fill all columns";
                alertColor = Radzen.AlertStyle.Warning;
                showAlert = true; // Show alert
            }
            await RefreshTable();
        await    todoGrid.Reload();

        }

        private void CancelEdit(JobToDo client)
        {
            Reset(client);
            todoGrid.CancelEditRow(client);


        }

        private async Task DeleteRow(JobToDo jobToDo)
        {
            Reset(jobToDo);

            if (jobToDo.Id !=0)
            {
                var response = await OmService.DeleteJobTodo(jobToDo.Id);
                response.EnsureSuccessStatusCode();
                responseMessage = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == true)
                {
                    alertColor = Radzen.AlertStyle.Success;
                }
                else
                {
                    alertColor = Radzen.AlertStyle.Danger;

                }
                showAlert = true; // Show alert
             
                await RefreshTable();
            }
            else
            {
                todoGrid.CancelEditRow(jobToDo);
                await RefreshTable();
            }
        }

        private async Task InsertRow()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var client = new JobToDo { JobPostedDateTime = DateTime.UtcNow };
            todoToInsert.Add(client);
            await todoGrid.InsertRow(client);
        }

        private async void OnCreateRow(JobToDo client)
        {

            var response = await OmService.AddJobTodo(client);
            todoToInsert.Remove(client);
            response.EnsureSuccessStatusCode();
            responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == true)
            {
                alertColor = Radzen.AlertStyle.Success;
            }
            else
            {
                alertColor = Radzen.AlertStyle.Danger;

            }
            showAlert = true; // Show alert
            await RefreshTable();
        }


        private async Task ShowDialogChanged(bool value)
        {
            showDialog = value;
            StateHasChanged();
        }
        private async Task RefreshTable()
        {
            filteredtodos = await OmService.GetJobToDos();
            await todoGrid.Reload();
        }
        private void ShowDialog()
        {
            showDialog = true;
        }




        private async Task AddJob(JobToDoViewModel jobToDoViewModel)
        {

        }

        private async Task HandleFileUpload(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {



                newJobViewModel.Images.Add((IFormFile?)file);
            }
        }
        public async Task GeneratePdfFromDataGridAsync()
        {

            if (selectedClientId == 0)
            {
                responseMessage = "Please First select Client";
                alertColor = Radzen.AlertStyle.Warning;
                showAlert = true; // Show alert
                return;
            }
            try
            {
                var pdfBytes = await _pdfService.GetTodoDetailsPdfByClientId(selectedClientId);


                await jsRuntime.InvokeVoidAsync("BlazorDownloadFile", "OffsetPrint.pdf", "application/pdf", pdfBytes);
            }
            catch (Exception ex) { }
        }

        public async Task SendBulkWorkEmailByClientId()
        {



            var response = await _pdfService.SendBulkTodoEmailByClientId(selectedClientId);

            if (response.IsSuccessStatusCode == true)
            {
                responseMessage = await response.Content.ReadAsStringAsync();
                alertColor = Radzen.AlertStyle.Success;
                showAlert = true; // Show alert
            }
            else
            {
                responseMessage = await response.Content.ReadAsStringAsync();
                alertColor = Radzen.AlertStyle.Danger;
                showAlert = true; // Show alert

            }
        }
        private async void OnCellClick(DataGridCellMouseEventArgs<JobToDo> args)
        {
            if (todoToUpdate.Any())
            {
                editedFields.Add(new(todoToUpdate.First().Id, columnEditing));
            }

            columnEditing = args.Column.Property;

            await EditRow(args.Data);
        }
        private void OnPriceKeyDown(KeyboardEventArgs args, JobToDo toDo)
        {
            if (args.Key == "Tab" || args.Key == "Enter")
            {
                CalculateTotal(toDo);
            }
        }
        private void OnPaidAmountKeyDown(KeyboardEventArgs args, JobToDo toDo)
        {
            if (args.Key == "Tab" || args.Key == "Enter")
            {
                toDo.DueBalance = toDo.TotalPayable - toDo.PaidAmount;

                StateHasChanged();
            }
        }
        private async void CalculateTotal(JobToDo todo)
        {
            todo.TotalPayable = (int?)(todo.Quantity * todo.Price);
            todo.DueBalance = todo.TotalPayable - todo.PaidAmount;
            StateHasChanged();

        }
        int? CalculateTotalTotal()
        {
            int? totalTotal = 0;
            totalTotal = filteredtodos.Sum(x => x.TotalPayable);


            return totalTotal;
        }
        int? CalculateTotalPaidAmount()
        {
            int? totalTotal = 0;
            totalTotal = filteredtodos.Sum(x => x.PaidAmount);



            return totalTotal;
        }
        int? CalculateTotalDueAmount()
        {
            int? totalTotal = 0;
            totalTotal = filteredtodos.Sum(x => x.DueBalance);



            return totalTotal;
        }

    }
}
