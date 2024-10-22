using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using System.Security.Claims;

namespace RealStats.Controllers
{
    public class InboxManagerController : Controller
    {
        private readonly RealStateContext _context;

        public InboxManagerController(RealStateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentManager = _context.Managers.FirstOrDefault(m => m.UserId == userId);

            if (currentManager == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch only the inbox requests for the logged-in manager
            var inboxRequests = _context.InboxManagers
                .Where(i => i.ManagerId == currentManager.Id) 
                .Include(i => i.Manager)
                .Include(i => i.LeaseAgreement)
                .ThenInclude(l => l.Files)
                .ToList();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(inboxRequests);
        }


        public IActionResult OpenContractFile(int id)
        {
            var file = _context.ContractFiles
                .FirstOrDefault(f => f.LeaseAgreementId == id);

            if (file != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "contracts", file.FileName);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file to the browser to be opened
                return File(fileBytes, "application/pdf", file.FileName);
            }
            return NotFound(); // File not found
        }

    }
}
