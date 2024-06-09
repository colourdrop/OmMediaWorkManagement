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

        public async Task<string> AddClient(OmClient client)
        {
            var response = await httpClient.PostAsJsonAsync("/prod/api/OmMedia/AddClient", client);
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
            return await httpClient.GetFromJsonAsync<List<OmClient>>("/prod/api/OmMedia/GetAllClients");
             
        }

        public async Task<string> UpdateClient(OmClient client)
        {
            var response = await httpClient.PutAsJsonAsync($"/prod/api/OmMedia/UpdateClient/{client.Id}", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
