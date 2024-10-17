using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStats.Data;
using RealStats.Models;

namespace RealStats.Controllers;

public class PaymentController: Controller
{   
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RealStateContext _context;
    public PaymentController(RealStateContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    } 
    
    
    [Authorize]
    public IActionResult Tenant()
    {
        // get all reports to all current property that have a valid lease agreement
        var user = _userManager.GetUserAsync(User).Result;
        if (!user.IsManager)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    public IActionResult Manager()
    {
        // get all reports to all current property that have a valid lease agreement
        var user = _userManager.GetUserAsync(User).Result;
        if (user.IsManager)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
}