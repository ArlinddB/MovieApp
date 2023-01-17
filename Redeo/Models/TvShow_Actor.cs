namespace Redeo.Models
{
    public class TvShow_Actor
    {
        public int TvShowId { get; set; }
        public TvShow TvShow { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
