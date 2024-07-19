using DinkToPdf;
using DinkToPdf.Contracts;
using System.Net.Http;
namespace OmMediaWorkManagement.Web.Helper
{
    public class PdfService : IPdfService
    {
        private readonly HttpClient httpClient;
        public PdfService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<byte[]> GetTodoDetailsPdfByClientId(int clientId)
        {
            return await httpClient.GetFromJsonAsync<byte[]>($"/omapi/api/OmMedia/GetTodoDetailsPdfByClientId?clientId={clientId}");
        }

        public async Task<byte[]> GetWorkDetailsPdfByClientId(int clientId)
        {
            return await httpClient.GetFromJsonAsync<byte[]>($"/omapi/api/OmMedia/GetWorkDetailsPdfByClientId?clientId={clientId}");
        }

        public async Task<HttpResponseMessage> SendBulkTodoEmailByClientId(int clientId)
        {
            var response = await httpClient.PostAsync($"/omapi/api/OmMedia/SendBulkTodoEmailByClientId?clientId={clientId}", null);

            return response;
        }

        public async Task<HttpResponseMessage> SendBulkWorkEmailByClientId(int clientId)
        {
            var response = await httpClient.PostAsync($"/omapi/api/OmMedia/SendBulkWorkEmailByClientId?clientId={clientId}", null);

            return response;

        }

    }

}
