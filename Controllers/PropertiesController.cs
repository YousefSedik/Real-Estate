using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models;
using System.Threading.Tasks;
using X.PagedList;

namespace RealStats.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly RealStateContext _context;

        public PropertiesController(RealStateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? page)
        {
            var properties = from p in _context.Properities
                             select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                properties = properties.Where(p => p.Name.Contains(searchString) 
                                                || p.Description.Contains(searchString)
                                                || p.City.Contains(searchString)
                                                || p.Country.Contains(searchString));
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var pagedProperties = await properties.Include(p => p.Images).Include(p => p.manager).ToListAsync();
            return View(pagedProperties.ToPagedList(pageNumber, pageSize));
        }

        public new IActionResult View(object value)
        {
            throw new NotImplementedException();
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