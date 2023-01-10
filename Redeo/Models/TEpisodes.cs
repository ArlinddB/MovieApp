using System.ComponentModel.DataAnnotations;

namespace Redeo.Models
{
    public class TEpisodes
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Episode { get; set; }
        public TSeason TSeason { get; set; }
        public int TvShowId { get; set; }
    }
}
