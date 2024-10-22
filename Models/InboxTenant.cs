using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class InboxTenant
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant tenant { get; set; }
        public int? LeaseAgreementId { get; set; }
        [ForeignKey("LeaseAgreementId")]
        public LeaseAgreement LeaseAgreement { get; set; }
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Properity Property { get; set; }
    }
}
