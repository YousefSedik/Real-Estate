using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
    
    
    [Authorize, Route("Payment/{leaseAgreementId:int}")]
    public async Task<IActionResult> Index(int leaseAgreementId)
    {
        var user = await _userManager.GetUserAsync(User);
        LeaseAgreement last_lease_agreement_valid = new LeaseAgreement();
        if (user.IsManager)
        {
            last_lease_agreement_valid = _context.LeaseAgreement.FirstOrDefault(l => l.Id == leaseAgreementId);
            if (last_lease_agreement_valid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        else
        {
            last_lease_agreement_valid = _context.LeaseAgreement
                .Include(l => l.Tenant)
                .FirstOrDefault(l => l.Id == leaseAgreementId);
            if (last_lease_agreement_valid == null || last_lease_agreement_valid.Tenant.UserId != user.Id)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        ViewData["lease_agreement"] = last_lease_agreement_valid;
        ViewData["payments"] = _context.Payment
            .Where(p => p.LeaseAgreementId == leaseAgreementId && p.StartDate <= DateTime.Now)
            .OrderByDescending(p => p.IsPaid)
            .ToList();
        var bill_paymetns = _context.Payment.Where(
            p => p.LeaseAgreementId == leaseAgreementId && !p.IsPaid && p.StartDate <= DateTime.Now
        ).ToList();
        ViewData["bill_payment"] = bill_paymetns;
        ViewData["total_amount"] = bill_paymetns.Sum(p => p.Amount);
        ViewData["is_manager"] = (user.IsManager);
        return View();
    }
    public async Task<IActionResult> Pay()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user.IsManager)
        {
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }
}