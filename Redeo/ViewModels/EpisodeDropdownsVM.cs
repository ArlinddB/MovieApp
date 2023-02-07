using Redeo.Models;

namespace Redeo.ViewModels
{
    public class EpisodeDropdownsVM
    {
        public EpisodeDropdownsVM()
        {
            Seasons = new List<TSeason>();
            TvShows = new List<TvShow>();
        }
        public List<TSeason> Seasons { get; set; }
        public List<TvShow> TvShows { get; set; }
    }
}
