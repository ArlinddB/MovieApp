using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class TSeason : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Season name is required")]
        [Display(Name = "Season name")]
        public string Season { get; set; }
        [Required(ErrorMessage = "Tv show name is required")]
        public string SeasonPoster { get; set; }
        [Required(ErrorMessage = "Season poster is required")] 
        public int TvShowId { get; set; }
        [ForeignKey("TvShowId")]
        public TvShow TvShow { get; set; }
        public List<TEpisodes> Episodes { get; set; }
    }
}
