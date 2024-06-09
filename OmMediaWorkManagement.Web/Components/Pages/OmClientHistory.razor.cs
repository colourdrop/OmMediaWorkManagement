using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using Radzen;
using Radzen.Blazor;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class OmClientHistory
    {
        [Inject]
        public IOmService OmService { get; set; }
        private bool showDialog = false;
        private string responseMessage = "";

        private RadzenDataGrid<OmClient> clientsGrid;
        public List<OmClient> clients { get; set; } = new List<OmClient>();


        private List<OmClient> clientsToInsert = new List<OmClient>();
        private List<OmClient> clientsToUpdate = new List<OmClient>();
        private DataGridEditMode editMode = DataGridEditMode.Single;
        private bool IsFirstRender { get; set; } = true;
        private int GetRowIndex(OmClient client)
        {
            return clients.IndexOf(client);
        }
        private void Reset()
        {
            clientsToInsert.Clear();
            clientsToUpdate.Clear();
        }

        private void Reset(OmClient client)
        {
            clientsToInsert.Remove(client);
            clientsToUpdate.Remove(client);
        }

        protected override async Task OnInitializedAsync()
        {
             
            clients = await OmService.GetAllClients();
        }

        private async Task EditRow(OmClient client)
        {
            if (editMode == DataGridEditMode.Single && clientsToInsert.Count() > 0)
            {
                Reset();
            }

            clientsToUpdate.Add(client);
            await clientsGrid.EditRow(client);
        }

        private void OnUpdateRow(OmClient client)
        {
            Reset(client);
          var result= OmService.UpdateClient(client);
        }

        private async Task SaveRow(OmClient client)
        {
            await clientsGrid.UpdateRow(client);
        }

        private void CancelEdit(OmClient client)
        {
            Reset(client);
            clientsGrid.CancelEditRow(client);

            // Assuming OmService.GetClient returns a client by ID
            //var clientEntry = OmService.GetClient(client.ClientID).Result;
            //if (clientEntry != null)
            //{
            //    client = clientEntry;
            //}
        }

        private async Task DeleteRow(OmClient client)
        {
            Reset(client);

            if (clients.Contains(client))
            {
             var result=await OmService.DeleteClient(client.Id);
                clients = clients.Where(c => c.Id != client.Id).ToList();
                await clientsGrid.Reload();
            }
            else
            {
                clientsGrid.CancelEditRow(client);
                await clientsGrid.Reload();
            }
        }

        private async Task InsertRow()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var client = new OmClient { CreatedAt = DateTime.UtcNow };
            clientsToInsert.Add(client);
            await clientsGrid.InsertRow(client);
        }

        private async void OnCreateRow(OmClient client)
        {

            var result = await OmService.AddClient(client);
            clientsToInsert.Remove(client);
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
            clients = await OmService.GetAllClients();
            await clientsGrid.Reload();
        }
        private void ShowDialog()
        {
            showDialog = true;
        }


    }
}
