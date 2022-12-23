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

        // Relationships

        [Display(Name = "Select categories")]
        [Required(ErrorMessage = "Movie categories are required")]
        public List<int> CategoryIds { get; set; }
    }
}