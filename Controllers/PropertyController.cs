using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models;

namespace RealStats.Controllers;
public class PropertyController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RealStateContext _context;

    public PropertyController(RealStateContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Details(int propertyId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); // Redirect if user is not authenticated
        }

        var property = await _context.Properities
            .Include(p => p.Images) // Ensure images are loaded
            .Include(p => p.Features) // Ensure features are loaded
            .Include(p => p.manager)
            .FirstOrDefaultAsync(p => p.Id == propertyId);

        if (property == null)
        {
            return NotFound(); // Return 404 if property is null
        }

        var tenant = await _context.Tenant.FirstOrDefaultAsync(t => t.UserId == user.Id);
    
        ViewData["property"] = property;
        ViewData["Images"] = property.Images?.ToList() ?? new List<Image>(); // Ensure not null
        ViewData["Features"] = property.Features?.ToList() ?? new List<Feature>(); // Ensure not null
        ViewData["IsManager"] = user.IsManager;
        //ViewData["redirect?"] = leaseAgreement == null && !user.IsManager;
        //ViewData["redirect"] = leaseAgreement == null && !user.IsManager ? "/" : null;
        //ViewData["lease_agreement"] = leaseAgreement;
        //ViewData["is_rented"] = leaseAgreement != null;
        //ViewData["pay_url"] = leaseAgreement != null ? "/payment/bill" : null;
        var manager_user = _context.Users.FirstOrDefault(u => u.Id == property.manager.UserId);
        ViewData["name"] = manager_user.Name;
        ViewData["email"] = manager_user.Email;
        ViewData["phone"] = manager_user.PhoneNumber;
        return View();
    }

}


// /Property/Owned => Manager, show all owned properties
// /Property/Rented => Manager, show properties owned and have valid lease agreement
// /Property/Rented => Tenant, show properties rented and have valid lease agreement 
// /Property/list => Tenant, show all properties with invalid lease agreement
// /Property/<id> => Tenant, show property, lease-agreement, report issue btn 
// /Property/<id> => Manager, show property, lease-agreement


// /payment => Tenant, bill then history then pay 
// /payment => Manager, 
// /payment/history => 
// /payment/bill => Manager