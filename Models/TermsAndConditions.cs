using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class TermsAndConditions
    {
        public int Id { get; set; }

        [Required]
        public string Terms { get; set; }

        [MaxLength(500)]
        public string PaymentTerms { get; set; }

        [MaxLength(500)]
        public string PenaltyClauses { get; set; }

        [MaxLength(500)]
        public string MaintenanceResponsibility { get; set; }

        [MaxLength(500)]
        public string CancellationPolicy { get; set; }

        [MaxLength(500)]
        public string RenewalPolicy { get; set; }

        [MaxLength(500)]
        public string InsuranceRequirements { get; set; }
        public int ProperityId { get; set; }

        [ForeignKey("ProperityId")]
        public Properity Properity { get; set; }
    }
}

//https://chatgpt.com/share/6710e58e-e304-800d-bc9d-72dabd06d532
