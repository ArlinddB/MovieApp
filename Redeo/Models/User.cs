using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class User
    {
        [Key]
        public string U_Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Birthdate is Required!")]
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }
        public List<FavoriteMovie> FavoriteMovies { get; set; }
        public List<FavoriteTvShow> FavoriteTvShows { get; set; }
    }
}
