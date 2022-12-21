using Redeo.Models;

namespace Redeo.ViewModels
{
    public class MovieDropdownsVM
    {
        public MovieDropdownsVM()
        {
            Categories = new List<Category>();
        }

        public List<Category> Categories { get; set; }
    }
}
