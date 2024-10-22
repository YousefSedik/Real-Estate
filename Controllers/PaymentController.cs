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

    [Authorize, Route("pay/all")]
    public async Task<IActionResult> PayAll()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user.IsManager)
        {
            return RedirectToAction("Index", "Home");
        }
        var tenant = _context.Tenant.FirstOrDefault(t => t.UserId == user.Id);
        var all_unpaid_payments = _context.Payment
            .Where(p => p.LeaseAgreement.TenantId == tenant.Id && !p.IsPaid)
            .Include(p => p.LeaseAgreement)  // Eager load LeaseAgreement if needed
            .ToList();
        
        foreach (var payment in all_unpaid_payments)
        {
            payment.IsPaid = true;
            payment.PaymentDate = DateTime.Now;
            _context.Update(payment);
        }
        await _context.SaveChangesAsync();

        return RedirectToAction("MyBills", "Payment");
    }
    
    [Authorize, Route("pay/contract/{leaseAgreementId:int}")]
    public async Task<IActionResult> PayContract(int leaseAgreementId)
    {
        // get all payments that are not paid
        var user = await _userManager.GetUserAsync(User);
        LeaseAgreement leaseAgreement = _context.LeaseAgreement
            .Include(l => l.Tenant)
            .FirstOrDefault(l => l.Id == leaseAgreementId);
        if (leaseAgreement == null || leaseAgreement.Tenant.UserId != user.Id)
        {
            return RedirectToAction("Index", "Home");
        }
        var payments = _context.Payment
            .Where(p => p.LeaseAgreementId == leaseAgreementId && !p.IsPaid)
            .ToList();
        foreach (var payment in payments)
        {
            payment.IsPaid = true;
            payment.PaymentDate = DateTime.Now;
            _context.Update(payment);
            await _context.SaveChangesAsync();
            
        }
        return RedirectToAction("Index", "Payment", new { leaseAgreementId = leaseAgreementId });

    }
    [Authorize, Route("pay/{leaseAgreementId:int}/{paymentId:int}")]
    public async Task<IActionResult> Pay(int paymentId, int leaseAgreementId, string next)
    {
        var user = await _userManager.GetUserAsync(User);
        Payment payment = _context.Payment
            .Include(p => p.LeaseAgreement)
            .ThenInclude(l => l.Tenant)
            .FirstOrDefault(p => p.Id == paymentId);
        if (payment == null || payment.LeaseAgreement.Tenant.UserId != user.Id)
        {
            return RedirectToAction("Index", "Home");
        }
        payment.IsPaid = true;
        payment.PaymentDate = DateTime.Now;
        _context.Update(payment);
        await _context.SaveChangesAsync();
        if (next == "my-bills")
        {
            return RedirectToAction("MyBills", "Payment");
        }
        return RedirectToAction("Index", "Payment", new { leaseAgreementId = leaseAgreementId });
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
        ViewData["lease_agreement_id"] = leaseAgreementId;
        ViewData["is_manager"] = (user.IsManager);
        return View();
    }
    [Authorize, Route("my-bills")]
    public async Task<IActionResult> MyBills()
    {
        var user = await _userManager.GetUserAsync(User);
        var tenant = _context.Tenant.FirstOrDefault(t => t.UserId == user.Id);
        if (tenant == null)
        {
            return RedirectToAction("Index", "Home");
        }
        var all_valid_lease_agreement = _context.LeaseAgreement
            .Where(l => l.Tenant.Id == tenant.Id && l.StartDate <= DateTime.Now)
            .Include(l => l.Tenant)  // Eagerly load Tenant if necessary
            .Include(l => l.Properity)  // Eagerly load Property
            .ToList();
        List<Payment> payments = new List<Payment>();
        foreach (var lease_agreement in all_valid_lease_agreement)
        {
            var payments_ = _context.Payment
                .Where(p => p.LeaseAgreementId == lease_agreement.Id && p.StartDate <= DateTime.Now)
                .Include(p => p.LeaseAgreement)
                .ThenInclude(l => l.Properity)
                .ToList();
            payments.AddRange(payments_);
        }
        ViewData["payments"] = payments;
        var bill_payment = payments.Where(p => !p.IsPaid).ToList();
        ViewData["bill_payment"] = bill_payment;
        ViewData["total_amount"] = bill_payment.Where(p => !p.IsPaid).Sum(p => p.Amount);
        return View();

    }       
}