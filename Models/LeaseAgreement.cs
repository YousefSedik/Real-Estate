using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class LeaseAgreement
    {
        public int Id { get; set; }

        public bool LeaseStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int LeaseDuration { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public int ProperityId { get; set; }
        [ForeignKey("ProperityId")]
        public Properity Properity { get; set; }
        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public Manager Manager { get; set; }
        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; }
    }
}
