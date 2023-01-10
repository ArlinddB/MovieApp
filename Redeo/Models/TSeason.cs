using System.ComponentModel.DataAnnotations;

namespace Redeo.Models
{
    public class TSeason
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Season { get; set; }
        public List<TEpisodes> Episodes { get; set; }
    }
}
