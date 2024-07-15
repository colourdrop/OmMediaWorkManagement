using OmMediaWorkManagement.Web.Components.Models;
using OmMediaWorkManagement.Web.Components.ViewModels;

namespace OmMediaWorkManagement.Web.Components.Services
{
    public class OmServices : IOmService
    {
        private readonly HttpClient httpClient;

        public OmServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        #region Client
        public async Task<HttpResponseMessage> AddClient(OmClient client)
        {
            // var response = await httpClient.PostAsJsonAsync("/prod /api/OmMedia/AddClient", client);
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddClient", client);

            return response;
        }


        public async Task<HttpResponseMessage> DeleteClient(int clientId)
        {
            var response = await httpClient.DeleteAsync($"/api/OmMedia/DeleteClientById/{clientId}");

            return response;
        }

        public async Task<List<OmClient>> GetAllClients()
        {
            try
            {
                // return await httpClient.GetFromJsonAsync<List<OmClient>>("/prod /api/OmMedia/GetAllClients");
                return await httpClient.GetFromJsonAsync<List<OmClient>>("/api/OmMedia/GetAllClients");
            }
            catch(Exception ex) {
                throw ex;
            }
        }


        public async Task<HttpResponseMessage> UpdateClient(OmClient client)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/OmMedia/UpdateClient/{client.Id}", client);

            return response;
        }
        #endregion

        #region ClientWork

        public async Task<List<OmClientWork>> GetAllClientWork()
        {
            return await httpClient.GetFromJsonAsync<List<OmClientWork>>("/api/OmMedia/GetAllClientWork");
        }
        public async Task<List<OmClientWork>> GetClientWorkById(int clientID)
        {
            return await httpClient.GetFromJsonAsync<List<OmClientWork>>($"/api/OmMedia/GetWorksByClientId/{clientID}");
        }
        public async Task<HttpResponseMessage> AddClientWork(AddWorkViewModel clientWork)
        {
            // var response = await httpClient.PostAsJsonAsync("/prod /api/OmMedia/AddClient", client);
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddWork", clientWork);

            return response;
        }

        public async Task<HttpResponseMessage> UpdateClientPaymentWorkStatus(int clientId, int clientWorkId, bool isPaid)
        {
            // Construct the URL with parameters
            string url = $"/api/OmMedia/UpdatePaymentWorksStatusByClientId?clientId={clientId}&clientWorkId={clientWorkId}&isPaid={isPaid}";

            // Make HTTP PUT request
            var response = await httpClient.PutAsync(url, null);

            return response;
        }
        public async Task<HttpResponseMessage> UpdateClientWork(OmClientWork client)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/OmMedia/UpdateWork/{client.Id}", client);
            return response;
        }
        public async Task<HttpResponseMessage> DeleteClientWork(int clientWorkId, int omClientId)
        {

            var response = await httpClient.DeleteAsync($"/api/OmMedia/DeleteWorksByClientId?clientWorkId={clientWorkId}&omClientId={omClientId}");

            return response;
        }
        #endregion

        #region JobToDo

        public async Task<List<JobToDo>> GetJobToDos()
        {
            return await httpClient.GetFromJsonAsync<List<JobToDo>>("/api/OmMedia/GetJobToDoList");
        }
        public async Task<List<JobToDo>> GetJobsToDosByClientId(int clientID)
        {
            return await httpClient.GetFromJsonAsync<List<JobToDo>>($"/api/OmMedia/GetJobsToDosByClientId/{clientID}");
        }
        public async Task<HttpResponseMessage> AddJobTodo(JobToDo toDo)
        {
            // var response = await httpClient.PostAsJsonAsync("/prod /api/OmMedia/AddClient", client);
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddJobTodo", toDo);
            return response;
        }
        public async Task<HttpResponseMessage> UpdateJobtToDo(int id, JobToDoViewModel toDo)
        {
       
            var response = await httpClient.PutAsJsonAsync<JobToDoViewModel>(
                $"/api/OmMedia/UpdateJobTodo/{id}?OmClientId={toDo.OmClientId}&Quantity={toDo.Quantity}&Price={toDo.Price}&PaidAmount={toDo.PaidAmount}&DueBalance={toDo.DueBalance}&TotalPayable={toDo.TotalPayable}&total={toDo.total}&Description={toDo.Description}&IsStatus={toDo.IsStatus}&JobStatusType={toDo.JobStatusType}",
                null);
            return response;
        }
        public async Task<HttpResponseMessage> DeleteJobTodo(int id)
        {
            var response = await httpClient.DeleteAsync($"/api/OmMedia/DeleteJobTodo/{id}");

            return response;
        }



        #endregion


        #region GetJOB Status
        public async Task<List<JobTypeStatusViewModel>> GetJobTypeStatusList()
        {
            return await httpClient.GetFromJsonAsync<List<JobTypeStatusViewModel>>(" /api/OmMedia/GetJobTypeStatusList");
        }
        #endregion

        #region Send Notification
        public async Task<HttpResponseMessage> SendEmailByClientId(int clientId, int clientWorkId)
        {
            string url = $" /api/OmMedia/SendEmailByClientId?clientId={clientId}&clientWorkId={clientWorkId}";

            // Make HTTP PUT request
            var response = await httpClient.PostAsJsonAsync<Object>(url, null);

            // Check if the request was successful
            return response;
        }
        #endregion

        #region EmployeeManagement
        public async Task<List<OmEmployee>> GetOmEmployees()
        {
            return await httpClient.GetFromJsonAsync<List<OmEmployee>>(" /api/OmMedia/GetAllEmployee");
        }
        public async Task<List<OmEmployeeSalaryManagement>> GetSalaryManagementByEmployeeId(int employeeID)
        {
            return await httpClient.GetFromJsonAsync<List<OmEmployeeSalaryManagement>>($"/api/OmMedia/GetSalaryManagementByEmployeeId?OmEmployeeId={employeeID}");
        }

        public async Task<HttpResponseMessage> AddEmployee(AddOmEmployee addOmEmployee)
        {
            using (var formData = new MultipartFormDataContent())
            {
                // Add string content parameters
                formData.Add(new StringContent(addOmEmployee.Name ?? ""), "Name");
                formData.Add(new StringContent(addOmEmployee.Address ?? ""), "Address");
                formData.Add(new StringContent(addOmEmployee.CompanyName ?? ""), "CompanyName");
                formData.Add(new StringContent(addOmEmployee.Email ?? ""), "Email");
                formData.Add(new StringContent(addOmEmployee.PhoneNumber ?? ""), "PhoneNumber");
                formData.Add(new StringContent(addOmEmployee.SalaryAmount.ToString()), "SalaryAmount");
                formData.Add(new StringContent(addOmEmployee.IsSalaryPaid.ToString()), "IsSalaryPaid");
                formData.Add(new StringContent(addOmEmployee.Description ?? ""), "Description");

                // Add EmployeeProfile file if present
                if (addOmEmployee.EmployeeProfile != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await addOmEmployee.EmployeeProfile.CopyToAsync(stream);
                        formData.Add(new StreamContent(stream), "EmployeeProfile", addOmEmployee.EmployeeProfile.FileName);
                    }
                }
                // Add EmployeeDocuments files if present
                if (addOmEmployee.EmployeeDocuments != null && addOmEmployee.EmployeeDocuments.Any())
                {
                    foreach (var file in addOmEmployee.EmployeeDocuments)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            formData.Add(new StreamContent(stream), "EmployeeDocuments", file.FileName);
                        }
                    }
                }


                // Send HTTP POST request
                var response = await httpClient.PostAsync("/api/OmMedia/AddEmployee", formData);

                return response;
            }
        }

        public async Task<HttpResponseMessage> AddSalaryManagement(OmEmployeeSalaryManagement omEmployeeSalaryManagement)
        {
            var response = await httpClient.PostAsJsonAsync("/api/OmMedia/AddSalaryManagement", omEmployeeSalaryManagement);

            return response;
        }
        #endregion


    }
}
