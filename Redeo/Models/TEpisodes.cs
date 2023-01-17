using Redeo.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Models
{
    public class TEpisodes : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Episode { get; set; }
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public TSeason Season { get; set; }
    }
}
