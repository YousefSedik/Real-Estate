using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;

namespace RealStats.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RealStateContext _context;
    public HomeController(RealStateContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(HomeViewModel viewModel)
    {
        var properties = _context.Properities
            .Where(p => 
                p.City == viewModel.City &&
                (EF.Functions.Like(p.Street, "%" + viewModel.keyWord + "%") ||
                 EF.Functions.Like(p.Description, "%" + viewModel.keyWord + "%") ||
                 EF.Functions.Like(p.Name, "%" + viewModel.keyWord + "%") ||
                 EF.Functions.Like(p.Country, "%" + viewModel.keyWord + "%")
                 ))
            .ToList();

        // If features are selected, filter by features
        if (viewModel.SelectedFeatures != null && viewModel.SelectedFeatures.Count > 0)
        {
            properties = properties
                .Where(p => p.Features.Any(f => viewModel.SelectedFeatures.Contains(f.Id)))
                .ToList();
        }

        // Get the features to show on the form
        var features = _context.Features.ToList();

        viewModel.Properities = properties;
        viewModel.Features = features;
        viewModel.cities = _context.Properities.Select(p => p.City).Distinct().ToList();
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            viewModel.UserName = user.Name;
        }

        return View(viewModel);
    }


}
