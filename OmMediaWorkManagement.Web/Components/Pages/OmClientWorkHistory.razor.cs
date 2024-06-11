//using Microsoft.AspNetCore.Components;
//using OmMediaWorkManagement.Web.Components.Models;
//using OmMediaWorkManagement.Web.Components.Services;
//using Radzen;
//using Radzen.Blazor;

//namespace OmMediaWorkManagement.Web.Components.Pages
//{
//    public partial class OmClientWorkHistory
//    {
//        [Inject]
//        public IOmService OmService { get; set; } 

//        private RadzenDataGrid<OmClientWork> clientsWorkGrid;
//        public List<OmClientWork> ClientWorkHistory { get; set; } = new List<OmClientWork>();

//        public List<OmClient> clients { get; set; } = new List<OmClient>();
//        protected override async Task OnInitializedAsync()
//        {

//            ClientWorkHistory = await OmService.GetAllClientWork();
//            clients = await OmService.GetAllClients();
//        }



//        private List<OmClientWork> clientsWorkToInsert = new List<OmClientWork>();
//        private List<OmClientWork> clientsWorkToUpdate = new List<OmClientWork>();
//        private DataGridEditMode editMode = DataGridEditMode.Single;
//        private bool IsFirstRender { get; set; } = true;
//        private int GetRowIndex(OmClientWork client)
//        {
//            return ClientWorkHistory.IndexOf(client);
//        }
//        private void Reset()
//        {
//            clientsWorkToInsert.Clear();
//            clientsWorkToUpdate.Clear();
//        }

//        private void Reset(OmClientWork client)
//        {
//            clientsWorkToInsert.Remove(client);
//            clientsWorkToUpdate.Remove(client);
//        }
         

//        private async Task EditRow(OmClientWork client)
//        {
//            if (editMode == DataGridEditMode.Single && clientsWorkToInsert.Count() > 0)
//            {
//                Reset();
//            }

//            clientsWorkToUpdate.Add(client);
//            //await clientsWorkGrid.EditRow(client);
//        }

//        private void OnUpdateRow(OmClientWork client)
//        {
//            Reset(client);
//            //var result = OmService.UpdateClient(client);
//        }

//        private async Task SaveRow(OmClientWork client)
//        {
//            //await clientsWorkGrid.UpdateRow(client);
//        }

//        private void CancelEdit(OmClientWork client)
//        {
//            Reset(client);
//            //clientsWorkGrid.CancelEditRow(client);

//            //// Assuming OmService.GetClient returns a client by ID
//            //var clientEntry = OmService.GetClient(client.ClientID).Result;
//            //if (clientEntry != null)
//            //{
//            //    client = clientEntry;
//            //}
//        }

//        private async Task DeleteRow(OmClientWork client)
//        {
//            Reset(client);

//            //if (clients.Contains(client))
//            //{
//            //    var result = await OmService.DeleteClient(client.Id);
//            //    clients = clients.Where(c => c.Id != client.Id).ToList();
//            //    await clientsWorkGrid.Reload();
//            //}
//            //else
//            //{
//            //    clientsWorkGrid.CancelEditRow(client);
//            //    await clientsWorkGrid.Reload();
//            //}
//        }

//        private async Task InsertRow()
//        {
//            if (editMode == DataGridEditMode.Single)
//            {
//                Reset();
//            }

//            //var client = new OmClientWork { CreatedAt = DateTime.UtcNow };
//            //clientsToInsert.Add(client);
//            //await clientsWorkGrid.InsertRow(client);
//        }

//        private async void OnCreateRow(OmClientWork client)
//        {

//            //var result = await OmService.AddClient(client);
//            //clientsToInsert.Remove(client);
           
//            // Refresh the table after adding a new record
//            await RefreshTable();
//        }


//        private async Task ShowDialogChanged(bool value)
//        {
            
//            StateHasChanged();
//        }
//        private async Task RefreshTable()
//        {
//            clients = await OmService.GetAllClients();
//            await clientsWorkGrid.Reload();
//        }
       
//    }
//}
