namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class JobToDoResponseViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public double? Quantity { get; set; }
        public bool? IsStatus { get; set; }
        public int JobStatusType { get; set; }
        public string JobStatusName { get; set; }

        public DateTime? JobPostedDateTime { get; set; }
        public List<string>? Images { get; set; }
    }

}
