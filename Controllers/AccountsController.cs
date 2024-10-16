using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;
using System.Threading.Tasks;

public class AccountsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RealStateContext _context;

    public AccountsController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RealStateContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create ApplicationUser with Name and IsManager properties
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,            
                IsManager = model.UserType == "Manager"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.UserType == "Manager")
                {
                    var manager = new Manager
                    {
                        UserId = user.Id,
                    };
                    _context.Managers.Add(manager);
                }
                else if (model.UserType == "Tenant")
                {
                    var tenant = new Tenant
                    {
                        UserId = user.Id,
                    };
                    _context.Tenant.Add(tenant);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Accounts");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ViewData["LoginError"] = "Invalid login attempt. Please check your email and password.";
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


}
