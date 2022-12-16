using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class Producers : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage=" Producers name is Required!")]
        [Display(Name="Producers Name")]
        public string ProducersName { get; set; }
           
        [Required(ErrorMessage="Profile picture is Required!")]
        [Display(Name = " Profile Picture")]
        public string ProfilePicture { get; set; }

        [Required(ErrorMessage ="Biography is required!")]
        [Display(Name="Biography")]
        public string Biography { get; set; }

        [Required(ErrorMessage = "Birthdate is Required!")]
        [Display(Name= "Birthdate")]
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }

        public Producers()
        {

        }
        
    }
}
