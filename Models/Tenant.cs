using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealStats.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LeaseAgreement> LeaseAgreement { get; set; } 
        public ICollection<Properity> Properity { get; set; }   
        public ICollection<ReportIssue> ReportIssue { get; set; }
    }
}
