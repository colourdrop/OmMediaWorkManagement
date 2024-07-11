namespace OmMediaWorkManagement.Web.Helper
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfAsync(string htmlContent);
    }
}
