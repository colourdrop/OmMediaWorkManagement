using OmMediaWorkManagement.Web.Components.Models;

namespace OmMediaWorkManagement.Web.Components.Services
{
    public interface IOmService
    {
        #region Client
        Task<List<OmClient>> GetAllClients();
        Task<string> AddClient(OmClient client);
        Task<string> UpdateClient(OmClient client);
        Task<string> DeleteClient(int clientId);
        #endregion

        #region ClientWork
        Task<List<OmClientWork>> GetAllClientWork();
        Task<OmClientWork> GetClientWorkById();
        //Task<string> AddClient(OmClient client);
        //Task<string> UpdateClient(OmClient client);
        //Task<string> DeleteClient(int clientId);
        #endregion
    }
}
