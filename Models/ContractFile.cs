using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class ContractFile
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public DateTime UploadedAt { get; set; }

        public int LeaseAgreementId { get; set; }

        [ForeignKey("LeaseAgreementId")]
        public LeaseAgreement LeaseAgreement { get; set; }
    }
}
