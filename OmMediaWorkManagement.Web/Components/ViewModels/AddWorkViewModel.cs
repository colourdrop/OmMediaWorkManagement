namespace OmMediaWorkManagement.Web.Components.ViewModels
{
    public class AddWorkViewModel
    {
        public int ClientId { get; set; }
        public DateTime WorkDate { get; set; }
        public string WorkDetails { get; set; }
        public int PrintCount { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Remarks
        {
            get; set;
        }
    }
}
