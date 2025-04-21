using System.ComponentModel.DataAnnotations;

namespace PB503WebApp.ViewModels.Slider
{
	public class SliderCreateVM
	{
        [Required(ErrorMessage = "Title bos olmaz")]
        public string Title { get; set; }
        [Required]
       
		public string? Description { get; set; }

        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
	}
}
