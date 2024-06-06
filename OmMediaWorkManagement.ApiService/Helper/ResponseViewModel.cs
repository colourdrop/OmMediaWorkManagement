namespace OmMediaWorkManagement.ApiService.Helper;


    public class ResponseViewModel
    {
        public RequestStatusEnum Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }

