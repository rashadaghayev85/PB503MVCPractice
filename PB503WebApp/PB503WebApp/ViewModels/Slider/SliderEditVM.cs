using System.ComponentModel.DataAnnotations;

namespace PB503WebApp.ViewModels.Slider
{
    public class SliderEditVM
    {
        [Required]
        public string Title { get; set; }
    
        public string Description { get; set; }

        public IFormFile? NewImage { get; set; }
        public string? ImageUrl { get; set; }
    }
}
