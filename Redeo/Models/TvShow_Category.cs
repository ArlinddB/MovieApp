namespace Redeo.Models
{
    public class TvShow_Category
    {
        public int TvShowId { get; set; }
        public TvShow TvShow { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
