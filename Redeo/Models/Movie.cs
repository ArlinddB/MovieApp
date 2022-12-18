using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class Movie : IEntityBase
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

        // Relationship
        public List<Movie_Category> Movies_Categories { get; set; }

    }
}
