using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.ViewModels
{
    public class TvShowVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Picture is required")]
        public string TvShowPicture { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime DateOfRelease { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Quality type is required")]
        [MaxLength(10, ErrorMessage = "Max length is 10 characters")]
        public string Quality { get; set; }
        public int Clicks { get; set; }

        //Relationship
        [Display(Name = "Select a producer")]
        [Required(ErrorMessage = "Producer is required")]
        public int ProducerId { get; set; }

        [Display(Name = "Select categories")]
        [Required(ErrorMessage = "Categories are required")]
        public List<int> CategoryIds { get; set; }
        [Display(Name = "Select actors")]
        [Required(ErrorMessage = "Actors are required")]
        public List<int> ActorIds { get; set; }
    }
}
