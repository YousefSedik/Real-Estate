using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ProperityId { get; set; }
        [ForeignKey("ProperityId")]
        public Properity Properity { get; set; }
    }
}
