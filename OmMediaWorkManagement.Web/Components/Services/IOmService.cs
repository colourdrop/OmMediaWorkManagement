using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.ViewModels;

namespace OmMediaWorkManagement.Web.Components.Services
{
    public interface IOmService
    {
        #region Client
        Task<List<OmClient>> GetAllClients();
        Task<HttpResponseMessage> AddClient(OmClient client); 
        Task<HttpResponseMessage> UpdateClient(OmClient client);
        Task<HttpResponseMessage> DeleteClient(int clientId);
        #endregion

        #region ClientWork
        Task<List<OmClientWork>> GetAllClientWork();
        Task<List<OmClientWork>> GetClientWorkById(int clientID);
        Task<HttpResponseMessage> AddClientWork(AddWorkViewModel clientWork);
        Task<HttpResponseMessage> UpdateClientPaymentWorkStatus(int clientId, int clientWorkId, bool isPaid);
        Task<HttpResponseMessage> UpdateClientWork(OmClientWork client);
        Task<HttpResponseMessage> DeleteClientWork(int clientId,int clientWorkId);
        #endregion


        #region JOBtodoStatus
        Task<List<JobToDo>> GetJobToDos();
        Task<HttpResponseMessage> AddJobTodo(JobToDo client); 
        Task<HttpResponseMessage> UpdateJobtToDo(JobToDo client);


        #endregion


        #region GetJOB Status
       
        Task<List<JobTypeStatusViewModel>> GetJobTypeStatusList();
        #endregion

        #region Send Notification
        Task<HttpResponseMessage> SendEmailByClientId(int clientId, int clientWorkId);
        #endregion

        #region EmployeeManagement
        Task<List<OmEmployee>>GetOmEmployees();
        #endregion
    }
}
