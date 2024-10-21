using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;
using System.Security.Claims;

namespace RealStats.Controllers
{
    public class MyProperityController : Controller
    {
        private readonly RealStateContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyProperityController(RealStateContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        [Route("MyProperity")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);

            if (user.IsManager)
            {
                var manager = await _context.Managers.FirstOrDefaultAsync(m => m.UserId == userId);

                if (manager == null)
                {
                    ViewBag.Message = "Manager not found.";
                    return View(new List<MyProperityViewModel>());
                }

                var properties = await _context.Properities
                    .Include(p => p.Images)
                    .Where(p => p.ManagerId == manager.Id)
                    .Select(p => new MyProperityViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Image = p.Images.FirstOrDefault().ImageUrl,
                        status = p.Status
                    })
                    .ToListAsync();

                if (properties == null || !properties.Any())
                {
                    ViewBag.Message = "No properties found for this manager.";
                }

                return View(properties);
            }
            else
            {
                var tenant = await _context.Tenant.FirstOrDefaultAsync(t => t.UserId == userId);

                if (tenant == null)
                {
                    ViewBag.Message = "Tenant not found.";
                    return View(new List<MyProperityViewModel>());
                }

                var properties = await _context.Properities
                    .Include(p => p.Images)
                    .Include(p => p.LeaseAgreements)
                    .Where(p => p.TenantId == tenant.Id)
                    .Select(p => new MyProperityViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Image = p.Images.FirstOrDefault().ImageUrl,
                        LeaseAgreementId = p.LeaseAgreements.First().Id
                    })
                    .ToListAsync();

                if (properties == null || !properties.Any())
                {
                    ViewBag.Message = "No properties found for this tenant.";
                }

                return View(properties);
            }
        }
    }
}
