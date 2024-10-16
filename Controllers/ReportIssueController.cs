using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;

namespace RealStats.Controllers;

public class ReportIssueController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RealStateContext _context;
    public ReportIssueController(RealStateContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Route("ReportIssue/")]
    [Authorize]
    public IActionResult Index()
    {
        // get all reports to all current property that have a valid lease agreement
        var user = _userManager.GetUserAsync(User).Result;
        if (!user.IsManager)
        {
            return RedirectToAction("Index", "Home");
        }
        var valid_lease_aggreaments = _context.LeaseAgreement.Where(
            l => l.Manager.UserId == user.Id && l.EndDate > DateTime.Now).ToList();
        var reportIssues = new List<ReportIssue>();
        foreach (var lease in valid_lease_aggreaments)
        {
            var issues = _context.ReportIssues.Include(r => r.Tenant)
                .ThenInclude(t => t.User)  // Load the Tenant and its User
                .Include(r => r.Properity).Where(r => r.ProperityId == lease.ProperityId).ToList();
            reportIssues.AddRange(issues);
        }
        if (reportIssues == null || !reportIssues.Any())
        {
            reportIssues = new List<ReportIssue>();
        }
        ViewData["ReportIssues"] = reportIssues;
        return View();
    }

    [Route("ReportIssue/Create/{property_id}")]
    [Authorize]
    public async Task<IActionResult> Create(ReportIssueViewModel viewModel, int property_id)
    {
        // check if it's valid property id
        var property = _context.Properities.FirstOrDefault(p => p.Id == property_id);
        if (property == null)
        {
            return RedirectToAction("Index", "Home");
        }
        var user = await _userManager.GetUserAsync(User);


        if (user.IsManager)
        {
            return RedirectToAction("Index", "Home");
        }
        // make sure that the tenant have lease aggreament with the property
        // get the last lease aggreament
        var last_lease_aggreaments = _context.LeaseAgreement.Where(
            l => l.Tenant.UserId == user.Id && l.ProperityId == property_id).ToList();
        var last_lease_aggreament = last_lease_aggreaments.Count > 0 ? last_lease_aggreaments[last_lease_aggreaments.Count - 1] : null;
        if (last_lease_aggreament == null)
        {
            return RedirectToAction("Index", "Home");
        }
        // check if the lease is still valid
        if (last_lease_aggreament.EndDate < DateTime.Now)
        {
            return RedirectToAction("Index", "Home");
        }
        // Handle POST request
        if (Request.Method == "POST")
        {
            // Ensure model is valid
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Return the view with the model to show validation errors
            }
            var tenant = _context.Tenant.FirstOrDefault(t => t.UserId == user.Id);
            if (tenant == null)
            {
                // Handle tenant not found (optional)
                return RedirectToAction("Error", "Home"); // Redirect to an error page or appropriate action
            }

            var reportIssue = new ReportIssue
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                ProperityId = property_id,
                TenantId = tenant.Id
            };

            _context.ReportIssues.Add(reportIssue);
            await _context.SaveChangesAsync(); // Use async method for saving changes
            return RedirectToAction("Index", "Home");
        }

        // Return the view with the model for GET requests
        return View(viewModel);
    }


}