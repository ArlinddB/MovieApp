using System.ComponentModel.DataAnnotations;

namespace Redeo.ViewModels
{
    public class SeasonVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Season name is required")]
        [Display(Name = "Season name")]
        public string Season { get; set; }
        public int TvShowId { get; set; }
    }
}
