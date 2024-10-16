using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Payment() { 
        PaymentDate = DateTime.Now;
        }
        public int LeaseAgreementId { get; set; }
        [ForeignKey("LeaseAgreementId")]
        public LeaseAgreement LeaseAgreement { get; set; }
   
    }
}
