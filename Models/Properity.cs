using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealStats.Models
{
    public class Properity
    {
        public int Id { get; set; }

        [MaxLength(150), Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int Area { get; set; }

        [MaxLength(int.MaxValue), Required] 
        public string Description { get; set; }

        [Required]
        public uint Bedrooms { get; set; }

        [Required]
        public uint Bathrooms { get; set; }

        public uint Garages { get; set; }

        [Range(1, double.MaxValue), Required]
        public double Price { get; set; }
        public bool Status { get; set; }

        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public Manager manager { get; set; }

        public TermsAndConditions TermsAndConditions { get; set; }

        public ICollection<LeaseAgreement> LeaseAgreements { get; set; }    
        public ICollection<Tenant> Tenants { get; set; }

        public ICollection<ReportIssue> ReportIssues { get; set; }  
        public ICollection<Feature> Features   { get; set; }
        public ICollection<Image> Images { get; set; }

    }
}
