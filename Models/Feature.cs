using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Properity> Properities { get; set; }
    }
}
