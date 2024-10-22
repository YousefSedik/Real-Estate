using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealStats.Data;
using RealStats.Models;
using System.Threading.Tasks;

namespace RealStats.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly RealStateContext _context;

        public PropertiesController(RealStateContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index(string keyword, string city, string status, string priceRange, string areaRange)
        {
            var properties = _context.Properities.Include(p => p.Images).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                properties = properties.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(city))
            {
                properties = properties.Where(p => p.City == city);
            }

            if (!string.IsNullOrEmpty(status))
            {
                bool isAvailable = status.Equals("true", StringComparison.OrdinalIgnoreCase);
                properties = properties.Where(p => p.Status == isAvailable);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                var priceValues = priceRange.Split(',');
                if (priceValues.Length == 2 &&
                    double.TryParse(priceValues[0], out double minPrice) &&
                    double.TryParse(priceValues[1], out double maxPrice))
                {
                    properties = properties.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
                }
                else
                {
                    ModelState.AddModelError("priceRange", "Invalid price range format.");
                }
            }
            if (!string.IsNullOrEmpty(areaRange))
            {
                var areaValues = areaRange.Split(',');
                if (areaValues.Length == 2 &&
                    int.TryParse(areaValues[0], out int minArea) &&
                    int.TryParse(areaValues[1], out int maxArea))
                {
                    properties = properties.Where(p => p.Area >= minArea && p.Area <= maxArea);
                }
                else
                {
                    ModelState.AddModelError("areaRange", "Invalid area range format.");
                }
            }
            var searchViewModel = new SearchViewModel
            {
                Properities = properties.ToList(),
                Cities = _context.Properities.Select(p => p.City).Distinct().ToList()
            };

            return View(searchViewModel);
        }

    }
}