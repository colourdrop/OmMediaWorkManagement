using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using Radzen;
using Radzen.Blazor;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class JobsToDo
    {
        [Inject]
        public IOmService OmService { get; set; }
        private bool showDialog = false;
        private string responseMessage = "";

        private RadzenDataGrid<JobToDo> todoGrid;
        public List<JobToDo> todos { get; set; } = new List<JobToDo>();


        private List<JobToDo> todoToInsert = new List<JobToDo>();
        private List<JobToDo> todoToUpdate = new List<JobToDo>();
        private DataGridEditMode editMode = DataGridEditMode.Single;
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
          var result= OmService.UpdateJobtToDo(client);
        }

        private async Task SaveRow(JobToDo client)
        {
            await todoGrid.UpdateRow(client);
        }

        private void CancelEdit(JobToDo client)
        {
            Reset(client);
            todoGrid.CancelEditRow(client);

            // Assuming OmService.GetClient returns a client by ID
            //var clientEntry = OmService.GetClient(client.ClientID).Result;
            //if (clientEntry != null)
            //{
            //    client = clientEntry;
            //}
        }

        private async Task DeleteRow(JobToDo client)
        {
            Reset(client);

            if (todos.Contains(client))
            {
             var result=await OmService.DeleteClient(client.Id);
                todos = todos.Where(c => c.Id != client.Id).ToList();
                await todoGrid.Reload();
            }
            else
            {
                todoGrid.CancelEditRow(client);
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

            var result = await OmService.AddJobTodo(client);
            todoToInsert.Remove(client);
            responseMessage = result; // Assuming result is a string message
            showDialog = true;
            // Refresh the table after adding a new record
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
        

    }
}
