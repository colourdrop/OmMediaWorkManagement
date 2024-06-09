using OmMediaWorkManagement.Web.Components.Models;

namespace OmMediaWorkManagement.Web.Components.Services
{
    public interface IOmService
    {

        Task<IEnumerable<OmClient>> GetAllClients();
    }
}
