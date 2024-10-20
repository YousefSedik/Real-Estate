using System.ComponentModel.DataAnnotations;

namespace RealStats.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Properity> Properities { get; set; }
        public ICollection<LeaseAgreement> LeaseAgreements { get; set; }
        public ICollection<InboxManager> InboxManagers { get; set; }
    }
}
