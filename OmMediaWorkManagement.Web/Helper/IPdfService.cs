namespace OmMediaWorkManagement.Web.Helper
{
    public interface IPdfService
    {
        //Task<byte[]> GeneratePdfAsync(string htmlContent);
        Task<byte[]> GetWorkDetailsPdfByClientId(int clientId);
        Task<byte[]> GetTodoDetailsPdfByClientId(int clientId);
        Task<HttpResponseMessage> SendBulkWorkEmailByClientId(int clientId);
        Task<HttpResponseMessage> SendBulkTodoEmailByClientId(int clientId);
    }
}
