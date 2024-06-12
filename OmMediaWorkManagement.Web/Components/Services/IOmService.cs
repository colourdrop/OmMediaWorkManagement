﻿using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.ViewModels;

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
        Task<List<OmClientWork>> GetClientWorkById(int clientID);
        Task<string> AddClientWork(AddWorkViewModel clientWork);
        Task<string> UpdateClientPaymentWorkStatus(int clientId, int clientWorkId, bool isPaid);
        Task<string> UpdateClientWork(OmClientWork client);
       
        #endregion
    }
}
