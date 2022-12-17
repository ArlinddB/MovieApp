namespace Redeo.Models
{
    public class Movie_Category
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
