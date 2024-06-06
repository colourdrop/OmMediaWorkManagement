using System.ComponentModel.DataAnnotations;

namespace OmMediaWorkManagement.ApiService.ViewModels
{
    public class JobToDoViewModel
    {

        [Required(ErrorMessage = "Company Name is required")]
        public string ComapnyName { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public string Quantity { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile? Image { get; set; }
              
        public int? PostedBy { get; set; }

        public bool? JobIsRunning { get; set; }
        public bool? JobIsFinished { get; set; }
        public bool? JobIsHold { get; set; }
        public bool? JobIsDeclained { get; set; }
    }
}
