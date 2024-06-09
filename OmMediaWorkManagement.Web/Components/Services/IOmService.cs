using OmMediaWorkManagement.Web.Components.Models;

namespace OmMediaWorkManagement.Web.Components.Services
{
    public interface IOmService
    {

        Task<List<OmClient>> GetAllClients();
        Task<string> AddClient(OmClient client);
        Task<string> UpdateClient(OmClient client);
        Task<string> DeleteClient(int clientId);
    }
}
