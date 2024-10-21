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
          public async Task<IActionResult> Index(int page = 1, int pageSize = 9, string keyword = null, string city = null, string status = null, decimal? minPrice = null, decimal? maxPrice = null, int? minBeds = null, int? minBaths = null)
          {
              var query = _context.Properities
                  .Include(p => p.Images)
                  .Include(p => p.manager)
                  .AsQueryable();

              if (!string.IsNullOrEmpty(keyword))
                  query = query.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword));

              if (!string.IsNullOrEmpty(city))
                  query = query.Where(p => p.City == city);

              if (!string.IsNullOrEmpty(status))
                  query = query.Where(p => p.Status.ToString() == status);

              if (minPrice.HasValue)
                  query = query.Where(p => p.Price >= (double)minPrice.Value);

              if (maxPrice.HasValue)
                  query = query.Where(p => p.Price <= (double)maxPrice.Value);

              if (minBeds.HasValue)
                  query = query.Where(p => p.Bedrooms >= minBeds.Value);

              if (minBaths.HasValue)
                  query = query.Where(p => p.Bathrooms >= minBaths.Value);

              var totalProperties = await query.CountAsync();
              var totalPages = (int)Math.Ceiling(totalProperties / (double)pageSize);

              var properties = await query
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

              ViewBag.CurrentPage = page;
              ViewBag.TotalPages = totalPages;

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

        // Add Create, Edit, and Delete actions as needed
    }
}