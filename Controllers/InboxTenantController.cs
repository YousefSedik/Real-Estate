using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealStats.Controllers
{
    public class InboxTenantController : Controller
    {
        private readonly RealStateContext _context;

        public InboxTenantController(RealStateContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tenantId = await _context.Tenant
                .Where(t => t.UserId == userId)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            var inboxMessages = await _context.InboxTenant
                .Include(i => i.Property)
                .Where(i => i.TenantId == tenantId)
                .ToListAsync();

            return View(inboxMessages);
        }
    }
}
