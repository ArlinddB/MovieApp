using Redeo.Models;

namespace Redeo.ViewModels
{
    public class EpisodeDropdownsVM
    {
        public EpisodeDropdownsVM()
        {
            Seasons = new List<TSeason>();
        }
        public List<TSeason> Seasons { get; set; }
    }
}
