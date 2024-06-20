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

        private RadzenDataGrid<OmClient> clientsGrid = new RadzenDataGrid<OmClient>();
        public List<OmClient> clients { get; set; } = new List<OmClient>();


        private List<OmClient> clientsToInsert = new List<OmClient>();
        private List<OmClient> clientsToUpdate = new List<OmClient>();
        private DataGridEditMode editMode = DataGridEditMode.Multiple;
        private string columnEditing;
        private List<KeyValuePair<int, string>> editedFields = new List<KeyValuePair<int, string>>();

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
            // Fetch existing clients from OmService
            clients = await OmService.GetAllClients();

            // Ensure clients grid is refreshed
            if (clientsGrid != null)
            {
                await clientsGrid.Reload();
            }
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
            var result = OmService.UpdateClient(client);
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

            if (clientsToInsert.Contains(client))
            {
                // Remove from clientsToInsert list
                clientsToInsert.Remove(client);
                clients.Remove(client); // Remove from clients list as well, assuming clients is still tracking all clients
                await clientsGrid.Reload(); // Refresh grid to reflect removal
            }
            else if (clients.Contains(client))
            {
                // Delete from database if it's an existing client
                var result = await OmService.DeleteClient(client.Id);

                clients = clients.Where(c => c.Id != client.Id).ToList();
                await clientsGrid.Reload(); // Refresh grid after deletion

            }
            else
            {
                // Cancel edit for newly inserted row that hasn't been saved yet
                clientsGrid.CancelEditRow(client);
                clientsToInsert.Remove(client); // Remove from clientsToInsert list
                await clientsGrid.Reload(); // Refresh grid to reflect cancellation
            }
        }


        private async Task InsertRow(int numberOfRowsToAdd)
        {
            // Add new rows
            for (int i = 0; i < numberOfRowsToAdd; i++)
            {
                var client = new OmClient { CreatedAt = DateTime.UtcNow };
                clientsToInsert.Add(client);
                clients.Add(client); // Add at the end of the list
            }
            clients.Sort((c1, c2) => c2.CreatedAt.CompareTo(c1.CreatedAt));
            // Set all rows to edit mode
            foreach (var client in clients)
            {
                await clientsGrid.EditRow(client);
            }

            // Refresh the grid to reflect changes
            await clientsGrid.Reload();
        }


        private async Task SaveAllRecords()
        {
            // Save updated rows
            foreach (var client in clientsToUpdate)
            {
                OnUpdateRow(client);
            }

            // Save newly inserted rows
            foreach (var client in clientsToInsert)
            {
                OnCreateRow(client);
            }

            // Clear lists after saving
            clientsToUpdate.Clear();
            clientsToInsert.Clear();
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



        //private void OnCellClick(DataGridCellMouseEventArgs<OmClient> args)
        //{
        //    if (clientsToUpdate.Any())
        //    {
        //        editedFields.Add(new(clientsToUpdate.First().Id, columnEditing));
        //    }

        //    columnEditing = args.Column.Property;

        //    if (clientsToUpdate.Any())
        //    {
        //        OnUpdateRow(clientsToUpdate.First());
        //    }

        //    EditRow(args.Data);
        //}

        //private bool IsEditing(string columnName, OmClient client)
        //{
        //    return columnEditing == columnName && clientsToUpdate.Contains(client);
        //}




    }
}
