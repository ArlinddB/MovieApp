using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.ViewModels
{
    public class MovieVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        [Required(ErrorMessage = "Release date is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfRelease { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Quality type is required")]
        [MaxLength(10, ErrorMessage = "Max length is 10 characters")]
        public string Quality { get; set; }

        [Required(ErrorMessage = "Movie url is required")]
        public string MovieUrl { get; set; }

        // Relationships

        [Display(Name = "Select categories")]
        [Required(ErrorMessage = "Movie categories are required")]
        public List<int> CategoryIds { get; set; }

        [Display(Name = "Select a producer")]
        [Required(ErrorMessage = "Movie producer is required")]
        public int ProducerId { get; set; }

        [Display(Name = "Select actors")]
        [Required(ErrorMessage = "Movie actors are required")]
        public List<int> ActorIds { get; set; }
    }
}