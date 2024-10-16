using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class ReportIssue
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(200), Required]
        public string Title { get; set; }
        [MaxLength(int.MaxValue), Required]

        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportIssue()
        {
            ReportDate = DateTime.Now;
        }
        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; }

        public int ProperityId { get; set; }
        [ForeignKey("ProperityId")]
        public Properity Properity { get; set; }

    }
}
