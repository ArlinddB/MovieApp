using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class Actor : IEntityBase
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Picture is required!")]
        [Display(Name = "Picture")]
        public string ProfilePictureURL { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Birthdate is required!")]
        [Display(Name = "Birthdate")]

        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Biography is required!")]
        [Display(Name = "Biography")]
        public string Bio { get; set; }
    }
}
