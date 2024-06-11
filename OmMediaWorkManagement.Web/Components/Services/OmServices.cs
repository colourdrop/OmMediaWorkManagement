using OmMediaWorkManagement.Web.Components.Models;

namespace OmMediaWorkManagement.Web.Components.Services
{
    public class OmServices:IOmService
    {
        private readonly HttpClient httpClient;

        public OmServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region Client
        public async Task<string> AddClient(OmClient client)
        {
           // var response = await httpClient.PostAsJsonAsync("/prod/api/OmMedia/AddClient", client);
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddClient", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        public async Task<string> DeleteClient(int clientId)
        {
            var response = await httpClient.DeleteAsync($"/prod/api/OmMedia/DeleteClientById/{clientId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<OmClient>> GetAllClients()
        {
           // return await httpClient.GetFromJsonAsync<List<OmClient>>("/prod/api/OmMedia/GetAllClients");
            return await httpClient.GetFromJsonAsync<List<OmClient>>("/api/OmMedia/GetAllClients");
             
        }


        public async Task<string> UpdateClient(OmClient client)
        {
            var response = await httpClient.PutAsJsonAsync($"/prod/api/OmMedia/UpdateClient/{client.Id}", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region ClientWork

        public async Task<List<OmClientWork>> GetAllClientWork()
        {
            return await httpClient.GetFromJsonAsync<List<OmClientWork>>("/api/OmMedia/GetAllClientWork");
        }
        public async Task<OmClientWork> GetClientWorkById()
        {
            return await httpClient.GetFromJsonAsync<OmClientWork>("/api/OmMedia/GetAllClientWork");
        }
        #endregion
    }
}
