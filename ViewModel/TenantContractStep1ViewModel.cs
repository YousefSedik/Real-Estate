using System.ComponentModel.DataAnnotations;

namespace RealStats.ViewModel
{
    public class TenantContractStep1ViewModel
    {
        public string? TenantName { get; set; }

        [Required(ErrorMessage = "Please enter the lease duration.")]
        [Range(1, 120, ErrorMessage = "Lease duration must be between 1 and 120 months.")]
        public int LeaseDuration { get; set; }

        [Required(ErrorMessage = "Please enter your personal ID.")]
        [StringLength(20, ErrorMessage = "Personal ID cannot be longer than 20 characters.")]
        public string PersonalId { get; set; }

        public int PropertyId { get; set; }
        public int ManagerId { get; set; }

        public int TenantId { get; set; }
    }
}
