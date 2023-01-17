using Redeo.Models;

namespace Redeo.ViewModels
{
    public class TvShowDropdownsVM
    {
        public TvShowDropdownsVM()
        {
            Categories = new List<Category>();
            Producers = new List<Producers>();
            Actors = new List<Actor>();
        }

        public List<Category> Categories { get; set; }
        public List<Producers> Producers { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
