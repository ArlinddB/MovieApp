using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Areas.Admin.ViewModels
{
    public class UserVM
    {
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Birthdate is Required!")]
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
