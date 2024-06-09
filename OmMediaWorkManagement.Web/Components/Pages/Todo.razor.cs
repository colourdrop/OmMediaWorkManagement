using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using Radzen.Blazor;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class Todo
    {
        [Inject]
        public IOmService OmService { get; set; }
        private RadzenDataGrid<OmClient> clientsGrid;
        public IEnumerable<OmClient> clients { get; set; }

        private List<OmClient> clientsToInsert = new List<OmClient>();
        private List<OmClient> clientsToUpdate = new List<OmClient>();
        bool IsFirstRender { get; set; } = true;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                IsFirstRender = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (!IsFirstRender)
            {
                // Only insert a new row if it's not the first render
                var client = new OmClient { CreatedAt = DateTime.UtcNow };
                clientsToInsert.Add(client);
                await clientsGrid.InsertRow(client);
            }

            clients = (await OmService.GetAllClients()).ToList();
        }




        async Task EditRow(OmClient client)
        {
            clientsToUpdate.Add(client);
            await clientsGrid.EditRow(client);
        }

        void OnUpdateRow(OmClient client)
        {
            Reset(client);

           // OmService.UpdateClient(client); // Update client in the service

            clientsGrid.Reload();
        }

        async Task SaveRow(OmClient client)
        {
            await clientsGrid.UpdateRow(client);
        }

        void CancelEdit(OmClient client)
        {
            Reset(client);

            clientsGrid.CancelEditRow(client);
        }

        async Task DeleteRow(OmClient client)
        {
            Reset(client);

            if (clients.Contains(client))
            {
              //  await OmService.DeleteClient(client.Id); // Delete client in the service
                clients = clients.Where(c => c.Id != client.Id).ToList();

                await clientsGrid.Reload();
            }
            else
            {
                clientsGrid.CancelEditRow(client);
                await clientsGrid.Reload();
            }
        }

        async Task InsertRow()
        {
            var client = new OmClient { CreatedAt = DateTime.UtcNow };
            clientsToInsert.Add(client);
            await clientsGrid.InsertRow(client);
        }

        void OnCreateRow(OmClient client)
        {
           // OmService.CreateClient(client); // Create client in the service

            clientsToInsert.Remove(client);

            clients = clients.Append(client);
        }

        void Reset(OmClient client)
        {
            clientsToInsert.Remove(client);
            clientsToUpdate.Remove(client);
        }
    }
}
