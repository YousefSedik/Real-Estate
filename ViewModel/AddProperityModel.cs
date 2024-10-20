using System.ComponentModel.DataAnnotations;

namespace RealStats.ViewModel
{
    public class AddProperityModel
    {
        public int PropertyId { get; set; }
        [Required, StringLength(150)]
        public string Name { get; set; }
        [Required, StringLength(int.MaxValue)]
        public string Description { get; set; }
        [Required, Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Street { get; set; }
        [Required, Range(0, 600)]
        public uint Bathrooms { get; set; }
        [Required, Range(0, 600)]
        public uint Bedrooms { get; set; }
        [Required, Range(0, 600)]
        public uint Garages { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int Area { get; set; }
        public List<int> Features { get; set; }
        public IFormFile[] images { get; set; }

        // Adding Terms and Conditions fields
        [Required]
        public string Terms { get; set; }

        //public int LeaseDuration { get; set; } // in months or years

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
    }
}
