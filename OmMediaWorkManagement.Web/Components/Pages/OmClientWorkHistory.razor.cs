using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using Radzen;
using Radzen.Blazor;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class OmClientWorkHistory
    {
        [Inject]
        public IOmService OmService { get; set; }
        private int selectedClientId;
        private RadzenDataGrid<OmClientWork> clientsWorkGrid;
        public List<OmClientWork> ClientWorkHistory { get; set; } = new List<OmClientWork>();
        public List<OmClient> clients { get; set; } = new List<OmClient>();
        private List<OmClientWork> filteredClientWorkHistory = new List<OmClientWork>();

        protected override async Task OnInitializedAsync()
        {
            ClientWorkHistory = await OmService.GetAllClientWork();
            clients = await OmService.GetAllClients();
            filteredClientWorkHistory = ClientWorkHistory; // Initialize with all records
        }

        private List<OmClientWork> clientsWorkToInsert = new List<OmClientWork>();
        private List<OmClientWork> clientsWorkToUpdate = new List<OmClientWork>();
        private DataGridEditMode editMode = DataGridEditMode.Single;
        private bool IsFirstRender { get; set; } = true;

        private void Reset()
        {
            clientsWorkToInsert.Clear();
            clientsWorkToUpdate.Clear();
        }

        private void Reset(OmClientWork client)
        {
            clientsWorkToInsert.Remove(client);
            clientsWorkToUpdate.Remove(client);
        }

        private async Task EditRow(OmClientWork client)
        {
            if (editMode == DataGridEditMode.Single && clientsWorkToInsert.Count() > 0)
            {
                Reset();
            }

            clientsWorkToUpdate.Add(client);
            await clientsWorkGrid.EditRow(client);
        }

        private void OnUpdateRow(OmClientWork client)
        {
            Reset(client);
            //var result = OmService.UpdateClientWork(client);
        }

        private async Task SaveRow(OmClientWork client)
        {
            await clientsWorkGrid.UpdateRow(client);
        }

        private void CancelEdit(OmClientWork client)
        {
            Reset(client);
            clientsWorkGrid.CancelEditRow(client);

            // Assuming OmService.GetClient returns a client by ID
            // var clientEntry = OmService.GetClientWork(client.ClientId).Result;
            // if (clientEntry != null)
            // {
            //     client = clientEntry;
            // }
        }

        private async Task DeleteRow(OmClientWork client)
        {
            Reset(client);

            if (ClientWorkHistory.Contains(client))
            {
                // var result = await OmService.DeleteClientWork(client.ClientId);

                await clientsWorkGrid.Reload();
            }
            else
            {
                clientsWorkGrid.CancelEditRow(client);
                await clientsWorkGrid.Reload();
            }
        }

        private async Task InsertRow()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var newClientWork = new OmClientWork();
            clientsWorkToInsert.Add(newClientWork);
            await clientsWorkGrid.InsertRow(newClientWork);
        }

        private async Task OnCreateRow(OmClientWork client)
        {
            if (clientsWorkToInsert.Contains(client))
            {
                clientsWorkToInsert.Remove(client);
                // var result = await OmService.CreateClientWork(client);
                // if (result != null)
                // {
                //     ClientWorkHistory.Add(result);
                //     await clientsWorkGrid.Reload();
                // }
            }
        }

        private void OnClientSelected(object value)
        {
            selectedClientId = (int)value;
            if (selectedClientId > 0)
            {
                filteredClientWorkHistory = ClientWorkHistory.Where(c => c.Id == selectedClientId).ToList();
            }
            else
            {
                filteredClientWorkHistory = ClientWorkHistory;
            }
        }

        private string GetClientName(int clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId)?.Name ?? "Unknown Client";
        }

    }
}
