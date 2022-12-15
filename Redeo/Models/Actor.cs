using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class Actor : IEntityBase
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Actor's picture is required!")]
        [Display(Name = "Actor's Picture")]
        public string ProfilePictureURL { get; set; }

        [Required(ErrorMessage = "Actor's name is required!")]
        [Display(Name = "Actor's Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Actor's birthdate is required!")]
        [Display(Name = "Actor's birthdate")]
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Actor's biography is required!")]
        [Display(Name = "Actor's biography")]
        public string Bio { get; set; }
    }
}
