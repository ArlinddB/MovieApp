using Redeo.Models;

namespace Redeo.ViewModels
{
    public class SeasonDropdownsVM
    {
        public SeasonDropdownsVM()
        {
            TvShows = new List<TvShow>();
        }

        public List<TvShow> TvShows { get; set; }
    }
}
