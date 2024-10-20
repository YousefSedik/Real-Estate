using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class InboxManager
    {

        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }

        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public Manager Manager { get; set; }

        public int LeaseAgreementId { get; set; }
        [ForeignKey("LeaseAgreementId")]
        public LeaseAgreement LeaseAgreement { get; set; }

    }
}
