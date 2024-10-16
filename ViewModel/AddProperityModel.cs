using RealStats.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.Marshalling;

namespace RealStats.ViewModel
{
    public class AddProperityModel
    {
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

    }
}