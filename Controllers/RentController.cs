using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models; // Your ApplicationUser class
using RealStats.ViewModel;
using System.Threading.Tasks;
using SelectPdf;

namespace RealStats.Controllers
{
    public class RentController : Controller
    {
        private readonly RealStateContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentController(RealStateContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Step1(int PropertyId)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int TenantId = _context.Tenant
            .Where(t => t.UserId == user.Id)
            .FirstOrDefault()?.Id ?? 0;

            int ManagerId = _context.Properities.Where(p => p.Id == PropertyId).FirstOrDefault()?.ManagerId ?? 0;
            Console.WriteLine(PropertyId);
            if (!user.IsManager) // If the user is not a manager
            {
                var model = new TenantContractStep1ViewModel
                {
                    TenantName = user.Name,
                    PropertyId = PropertyId,
                    ManagerId = ManagerId,
                    TenantId = TenantId
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Step1(TenantContractStep1ViewModel model)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Find the Tenant by UserId
                var tenant = await _context.Tenant
                    .FirstOrDefaultAsync(t => t.UserId == user.Id);

                if (tenant == null)
                {
                    ModelState.AddModelError("", "Tenant not found.");
                    return View(model);
                }
                var leaseAgreement = new LeaseAgreement
                {
                    LeaseStatus = 0, // Initially set to "waiting"
                    LeaseDuration = model.LeaseDuration,
                    PersonalId = model.PersonalId,
                    ProperityId = model.PropertyId,
                    ManagerId = model.ManagerId,
                    TenantId = tenant.Id
                };
                _context.LeaseAgreement.Add(leaseAgreement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Step2", new { PropertyId = model.PropertyId });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Step2(int PropertyId)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Get the tenant associated with the logged-in user
            var tenant = await _context.Tenant.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (tenant == null)
                return RedirectToAction("Home");

            // Find the lease agreement for the tenant and property with status 'waiting'
            var leaseAgreement = await _context.LeaseAgreement
                .FirstOrDefaultAsync(l => l.ProperityId == PropertyId && l.TenantId == tenant.Id && l.LeaseStatus == 0);
            if (leaseAgreement == null)
            {
                return RedirectToAction("Step1", new { PropertyId });
            }

            // Fetch the property and include the manager information
            var property = await _context.Properities.FindAsync(PropertyId);
            if (property == null)
            {
                return NotFound();
            }

            // Get the ManagerId from the property
            var managerId = property.ManagerId;

            // Find the manager using ManagerId
            var manager = await _context.Managers
                        .Include(m => m.User) // Include User details
                        .FirstOrDefaultAsync(m => m.Id == managerId);
            if (manager == null)
            {
                return NotFound();
            }


            // Fetch the Terms and Conditions for the property
            var termsAndConditions = await _context.TermsAndConditions
                .FirstOrDefaultAsync(t => t.ProperityId == PropertyId);

            // Populate the ContractViewModel with the required data
            var contractViewModel = new ContractViewModel
            {
                TenantName = tenant.User.Name,
                TenantPhone = tenant.User.PhoneNumber,
                TenantEmail = tenant.User.Email,

                ManagerName = manager.User.Name,
                ManagerPhone = manager.User.PhoneNumber,
                ManagerEmail = manager.User.Email,

                TermsAndConditions = termsAndConditions,

                Properity = property,

                LeaseAgreement = leaseAgreement
            };

            return View(contractViewModel);
        }
    }
}
