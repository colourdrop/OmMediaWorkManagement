namespace OmMediaWorkManagement.Web.Helper
{
    public class PdfService:IPdfService
    {
        public async Task<byte[]> GeneratePdfAsync(string htmlContent)
        {
            //var outputStream = new MemoryStream();
            //HtmlConverter.ConvertToPdf(htmlContent, outputStream);
            //return outputStream.ToArray();
            return null;
            
        }
    }
}
