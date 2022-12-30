using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class Movie : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Picture is required")]
        public string MoviePicture{ get; set; }

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
        [MaxLength(10,ErrorMessage = "Max length is 10 characters")]
        public string Quality { get; set; }

        [Required(ErrorMessage = "Movie url is required")]
        public string MovieUrl { get; set; }

        // Relationship
        public List<Movie_Category> Movies_Categories { get; set; }

        public int ProducerId { get; set; }
        [ForeignKey("ProducerId")]
        public Producers Producers { get; set; }
        public List<Movie_Actor> Movies_Actors { get; set; }

    }
}
