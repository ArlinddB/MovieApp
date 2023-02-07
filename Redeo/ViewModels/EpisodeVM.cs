using System.ComponentModel.DataAnnotations;

namespace Redeo.ViewModels
{
    public class EpisodeVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = ("Episode Name is required"))]
        public string Episode { get; set; }
        [Required(ErrorMessage = "Episode url is required")]
        public string EpisodeUrl { get; set; }
        [Display(Name = "Episode Description")]
        [Required(ErrorMessage = "Episode descriptionn is required")]
        public string EpisodeDescription { get; set; }
        [Required(ErrorMessage = "Season is required")]
        public int SeasonId { get; set; }
        [Required(ErrorMessage = "Tv show is required")]
        public int TvShowId { get; set; }
    }
}
