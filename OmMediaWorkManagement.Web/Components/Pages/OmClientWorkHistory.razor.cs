using Microsoft.AspNetCore.Components;
using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Components.ViewModels;
using Radzen;
using Radzen.Blazor;
using static System.Net.WebRequestMethods;

namespace OmMediaWorkManagement.Web.Components.Pages
{
    public partial class OmClientWorkHistory
    {
        [Inject]
        public IOmService OmService { get; set; }
        private int selectedClientId;
        private bool IsFirstRender { get; set; } = true;
        private RadzenDataGrid<OmClientWork> clientsWorkGrid;
        public List<OmClientWork> ClientWorkHistory { get; set; } = new List<OmClientWork>();
        public List<OmClient> clients { get; set; } = new List<OmClient>();
        private List<OmClientWork> filteredClientWorkHistory = new List<OmClientWork>();         
        private List<OmClientWork> clientsWorkToInsert = new List<OmClientWork>();
        private List<OmClientWork> clientsWorkToUpdate = new List<OmClientWork>();
        private DataGridEditMode editMode = DataGridEditMode.Single;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            ClientWorkHistory = await OmService.GetAllClientWork();
            clients = await OmService.GetAllClients();
            filteredClientWorkHistory = ClientWorkHistory.ToList(); // Ensure a copy is made
        }
        private async Task OnClientSelected(object value)
        {
            selectedClientId = (int)value;
            if (selectedClientId != 0)
            {
                filteredClientWorkHistory = await OmService.GetClientWorkById(selectedClientId);
            }

            else
            {
                filteredClientWorkHistory = await OmService.GetAllClientWork();
            }
            await clientsWorkGrid.Reload(); // Reload the grid to display the new data

        }
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

        private async void OnUpdateRow(OmClientWork client)
        {
            Reset(client);
            var result =await OmService.UpdateClientWork(client);
            await LoadData();
        }

        private async Task SaveRow(OmClientWork client)
        {
             CalculateTotal(client);
        
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
            AddWorkViewModel addWorkViewModel = new AddWorkViewModel()
            {
                ClientId = client.OmClientId,
                WorkDate=client.WorkDate.ToUniversalTime(),
                PrintCount=client.PrintCount,
                Price=client.Price,
                Remarks=client.Remarks,
                Total=client.Total,
               WorkDetails=client.WorkDetails
            };
            if (clientsWorkToInsert.Contains(client))
            {
                clientsWorkToInsert.Remove(client);
                var result = await OmService.AddClientWork(addWorkViewModel);
                if (result != null)
                {
                    await LoadData();
                }
            }
        }
        public async Task UpdateIsPaidStatus(OmClientWork work, bool isPaid)
        { 
            var result = await OmService.UpdateClientPaymentWorkStatus(work.OmClientId,work.Id,isPaid);
            await LoadData();
            
        }
        public async Task SendEmail(OmClientWork work )
        {
            var result = await OmService.SendEmailByClientId(work.OmClientId,work.Id );
            await LoadData();

        }
        private async void CalculateTotal(OmClientWork work)
        {
            work.Total = work.PrintCount * work.Price;
          await clientsWorkGrid.UpdateRow(work);


        }
        //private void OnClientSelected(object value)
        //{
        //    selectedClientId = (int)value;
        //    if (selectedClientId > 0)
        //    {
        //        filteredClientWorkHistory = ClientWorkHistory.Where(c => c.Id == selectedClientId).ToList();
        //    }
        //    else
        //    {
        //        filteredClientWorkHistory = ClientWorkHistory;
        //    }
        //}

        private string GetClientName(int clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId)?.Name ?? "Unknown Client";
        }

    }
}
