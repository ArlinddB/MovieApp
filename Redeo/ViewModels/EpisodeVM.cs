using System.ComponentModel.DataAnnotations;

namespace Redeo.ViewModels
{
    public class EpisodeVM
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Episode Name")]
        [Required(ErrorMessage = "Episode Name is required")]
        public string Episode { get; set; }
        [Required(ErrorMessage = "Season is required")]
        public int SeasonId { get; set; }
        
    }
}
