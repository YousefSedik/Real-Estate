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

    [HttpGet]
    public async Task<IActionResult> Details(int propertyId)
    {
        var user = await _userManager.GetUserAsync(User);

        var property = await _context.Properities
            .Include(p => p.Images)
            .Include(p => p.Features)
            .Include(p => p.manager)
            .Include(p => p.LeaseAgreements)
            .FirstOrDefaultAsync(p => p.Id == propertyId);

        if (property == null)
        {
            return NotFound();
        }

        var activeLeaseAgreement = property.LeaseAgreements
            .Where(la => la.LeaseStatus == 3) 
            .OrderByDescending(la => la.EndDate) 
            .FirstOrDefault();
        ViewData["EndDate"] = activeLeaseAgreement?.EndDate.ToString("d");


        var properties = await _context.Properities.Include(p => p.Images).ToListAsync();
        ViewData["properties"] = properties;
        ViewData["property"] = property;
        ViewData["Images"] = property.Images?.ToList() ?? new List<Image>(); 
        ViewData["Features"] = property.Features?.ToList() ?? new List<Feature>(); 
        ViewData["IsManager"] = user?.IsManager;
        ViewData["Status"] = property.Status;
        var manager_user = _context.Users.FirstOrDefault(u => u.Id == property.manager.UserId);
        ViewData["name"] = manager_user.Name;
        ViewData["email"] = manager_user.Email;
        ViewData["phone"] = manager_user.PhoneNumber;
        return View();
    }

}