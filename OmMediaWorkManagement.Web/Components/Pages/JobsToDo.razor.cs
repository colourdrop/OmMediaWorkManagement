using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
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
        List<string> imageUrls = new List<string>();
        private bool showAddDialog = false;
        private string responseMessage = "";
        private Radzen.AlertStyle alertColor = Radzen.AlertStyle.Info;
        private bool showAlert = false;
        private RadzenDataGrid<JobToDo> todoGrid;
        private List<IFormFile> images = new List<IFormFile>();
        private JobToDoViewModel newJobViewModel = new JobToDoViewModel();

        public List<JobToDo> todos { get; set; } = new List<JobToDo>();
        public List<JobTypeStatusViewModel> jobTypeStatusViewModels = new List<JobTypeStatusViewModel>();
        private List<JobToDo> todoToInsert = new List<JobToDo>();
        private List<JobToDo> todoToUpdate = new List<JobToDo>();
        private DataGridEditMode editMode = DataGridEditMode.Multiple;
        private string columnEditing;
        private List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();

        private bool IsFirstRender { get; set; } = true;
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

            todos = await OmService.GetJobToDos();
            jobTypeStatusViewModels = await OmService.GetJobTypeStatusList();
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
                if (toDo.Id != 0&& toDo.Price!= null && toDo.Total!=null)
                {
                    JobToDoViewModel jobToDoViewModel = new JobToDoViewModel()
                    {
                        ClientName = toDo.ClientName,
                        ComapnyName = toDo.CompanyName,
                        Quantity = (int)toDo.Quantity,
                        Price = (int)toDo.Price,
                        total = toDo.Total,                       
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

            if (todos.Contains(jobToDo))
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
                todos = todos.Where(c => c.Id != jobToDo.Id).ToList();
                await todoGrid.Reload();
            }
            else
            {
                todoGrid.CancelEditRow(jobToDo);
                await todoGrid.Reload();
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
            todos = await OmService.GetJobToDos();
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
        private async void CalculateTotal(JobToDo todo)
        {
            todo.Total = (int?)(todo.Quantity * todo.Price);
            //await clientsWorkGrid.UpdateRow(work);
            StateHasChanged();

        }
        decimal CalculateTotalTotal()
        {
            decimal totalTotal = 0;

            // Check if todos is not null before iterating
            if (todos != null)
            {
                foreach (var item in todos)
                {
                    // Check if item.Total is not null before attempting to add its value
                    if (item.Total != null)
                    {
                        totalTotal += (int)item.Total;
                    }
                }
            }

            return totalTotal;
        }

    }
}
