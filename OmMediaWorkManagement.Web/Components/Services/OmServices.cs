using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.ViewModels;

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
            var response = await httpClient.DeleteAsync($"/api/OmMedia/DeleteClientById/{clientId}");
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
            var response = await httpClient.PutAsJsonAsync($"/api/OmMedia/UpdateClient/{client.Id}", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region ClientWork

        public async Task<List<OmClientWork>> GetAllClientWork()
        {
            return await httpClient.GetFromJsonAsync<List<OmClientWork>>("/api/OmMedia/GetAllClientWork");
        }
        public async Task<List<OmClientWork>> GetClientWorkById(int clientID )
        {
            return await httpClient.GetFromJsonAsync<List<OmClientWork>>($"/api/OmMedia/GetWorksByClientId/{clientID}");
        }
        public async Task<string> AddClientWork(AddWorkViewModel clientWork)
        {
            // var response = await httpClient.PostAsJsonAsync("/prod/api/OmMedia/AddClient", client);
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddWork", clientWork);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateClientPaymentWorkStatus(int clientId, int clientWorkId, bool isPaid)
        {
            // Construct the URL with parameters
            string url = $"/api/OmMedia/UpdatePaymentWorksStatusByClientId?clientId={clientId}&clientWorkId={clientWorkId}&isPaid={isPaid}";

            // Make HTTP PUT request
            var response = await httpClient.PutAsync(url, null);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read and return the response content
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the error
                throw new HttpRequestException($"Failed to update payment work status. Status code: {response.StatusCode}");
            }
        }
        public async Task<string> UpdateClientWork(OmClientWork client)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/OmMedia/UpdateWork/{client.Id}", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region JobToDo

        public async Task<List<JobToDo>> GetJobToDos()
        {
            return await httpClient.GetFromJsonAsync<List<JobToDo>>("/api/OmMedia/GetJobToDoList");
        }

        public async Task<string> AddJobTodo(JobToDo client)
        {
            // var response = await httpClient.PostAsJsonAsync("/prod/api/OmMedia/AddClient", client);
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddJobTodo", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> UpdateJobtToDo(JobToDo client)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/OmMedia/UpdateJobTodo/{client.Id}", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }




        #endregion


        #region GetJOB Status
        public async Task<List<JobTypeStatusViewModel>> GetJobTypeStatusList()
        {
            return await httpClient.GetFromJsonAsync<List<JobTypeStatusViewModel>>("/api/OmMedia/GetJobTypeStatusList");
        }
        #endregion

        #region Send Notification
        public async Task<string> SendEmailByClientId(int clientId,int clientWorkId)
        { 
            string url = $"/api/OmMedia/SendEmailByClientId?clientId={clientId}&clientWorkId={clientWorkId}";

            // Make HTTP PUT request
            var response = await httpClient.PutAsync(url, null);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read and return the response content
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the error
                throw new HttpRequestException($"Failed to update payment work status. Status code: {response.StatusCode}");
            }
        }
        #endregion




    }
}
