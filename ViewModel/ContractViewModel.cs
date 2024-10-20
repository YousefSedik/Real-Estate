using RealStats.Models;

namespace RealStats.ViewModel
{
    public class ContractViewModel
    {
        public string TenantName { get; set; }
        public string TenantPhone { get; set; }
        public string TenantEmail { get; set; }

        public string ManagerName { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerEmail { get; set; }
        public TermsAndConditions TermsAndConditions { get; set; }

        public Properity Properity { get; set; }

        public LeaseAgreement LeaseAgreement { get; set; }
    }
}
