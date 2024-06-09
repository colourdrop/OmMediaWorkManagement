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

        public async Task<IEnumerable<OmClient>> GetAllClients()
        {
            var sds= await httpClient.GetFromJsonAsync<OmClient[]>("/prod/api/OmMedia/GetAllClients");
            return sds;
        }
    }
}
