using Microsoft.AspNetCore.Mvc;
using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Redeo.Models
{
    public class Category : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        // Realtionship
        public List<Movie_Category> Movies_Categories { get; set; }
        public List<TvShow_Category> TvShows_Categories { get; set; }
    }
}
