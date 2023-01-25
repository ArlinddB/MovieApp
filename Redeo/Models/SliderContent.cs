using System.ComponentModel.DataAnnotations;

namespace Redeo.Models
{
    public class SliderContent
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Movie name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        public string Quality{ get; set; }
    }
}
