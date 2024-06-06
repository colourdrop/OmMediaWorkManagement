namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class JobToDoResponseViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public double? Quantity { get; set; }
        public bool? JobIsRunning { get; set; }
        public bool? JobIsDeclained { get; set; }
        public bool? JobIsFinished { get; set; }
        public bool? JobIsHold { get; set; }
        public DateTime? JobPostedDateTime { get; set; }
        public List<string>? Images { get; set; }
    }

}
