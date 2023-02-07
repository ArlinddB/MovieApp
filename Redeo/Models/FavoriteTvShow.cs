namespace Redeo.Models
{
    public class FavoriteTvShow
    {
        public int TvShowId { get; set; }
        public TvShow TvShow { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool isFavorite { get; set; } = false;
    }
}
