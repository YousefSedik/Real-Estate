
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 9, string orderBy = "property_date", string order = "ASC", string keyword = null, string city = null, decimal? minPrice = null, decimal? maxPrice = null, int? minBedrooms = null, int? minBathrooms = null)
        {
            var query = _context.Properities
                .Include(p => p.Images)
                .Include(p => p.manager)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword));

            if (!string.IsNullOrEmpty(city))
                query = query.Where(p => p.City.Contains(city));

            if (minPrice.HasValue)
                query = query.Where(p => (decimal)p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => (decimal)p.Price <= maxPrice.Value);

            if (minBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= minBedrooms.Value);

            if (minBathrooms.HasValue)
                query = query.Where(p => p.Bathrooms >= minBathrooms.Value);

            switch (orderBy)
            {
                case "property_date":
                    query = order == "ASC" ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
                    break;
                case "property_price":
                    query = order == "ASC" ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                    break;
            }

            var totalProperties = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalProperties / pageSize);

            var properties = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.OrderBy = orderBy;
            ViewBag.Order = order;

            return View(properties);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properities
                .Include(p => p.Images)
                .Include(p => p.manager)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }
    }
}