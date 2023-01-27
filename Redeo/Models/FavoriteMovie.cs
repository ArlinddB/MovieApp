using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class FavoriteMovie
    {
        [Required]
        public int MovieId { get; set; }
        //[ForeignKey("MovieId")]
        public Movie Movie { get; set; }
        [Required]
        public string UserId { get; set; }
        //[ForeignKey("UserId")]
        public User User { get; set; }
        public bool isFavorite { get; set; } = false;
    }
}
