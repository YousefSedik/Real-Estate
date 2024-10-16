using System.Collections.Generic;
using RealStats.Models;

namespace RealStats.ViewModel
{
    public class HomeViewModel
    {
        public string UserName { get; set; }
        public string keyWord { get; set; } 
        public string City { get; set; }
        public List<int> SelectedFeatures { get; set; }
        public List<Feature> Features { get; set; }
        public List<Properity> Properities { get; set; }
        public List<string> cities { get; set; }
    }
}