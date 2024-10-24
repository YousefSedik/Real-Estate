﻿using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;
using System.Security.Claims;

namespace RealStats.Controllers
{
    public class AddProperityController : Controller
    {
        private readonly RealStateContext _context;

        public AddProperityController(RealStateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Features = _context.Features.ToList();
            var model = new AddProperityModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(AddProperityModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var manager = _context.Managers.FirstOrDefault(m => m.UserId == userId);

                var property = new Properity
                {
                    Name = model.Name,
                    Country = model.Country,
                    City = model.City,
                    Street = model.Street,
                    Area = model.Area,
                    Description = model.Description,
                    Bedrooms = model.Bedrooms,
                    Bathrooms = model.Bathrooms,
                    Garages = model.Garages,
                    Price = model.Price,
                    Status = true,
                    manager = manager,
                    Features = new List<Feature>(), // Initialize collections
                    Images = new List<Image>()
                };

                // Adding selected features
                if (model.Features != null && model.Features.Count > 0)
                {
                    var selectedFeatures = _context.Features.Where(f => model.Features.Contains(f.Id)).ToList();
                    property.Features.AddRange(selectedFeatures);
                }

                // Handling image uploads
                if (model.images != null && model.images.Length > 0)
                {
                    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }

                    foreach (var image in model.images)
                    {
                        if (image.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var imagePath = Path.Combine(imagesPath, uniqueFileName);

                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                image.CopyTo(stream);
                            }

                            var img = new Image
                            {
                                ImageUrl = "/images/" + uniqueFileName,
                                Properity = property
                            };
                            property.Images.Add(img);
                        }
                    }
                }

                _context.Properities.Add(property);
                _context.SaveChanges();

                Console.WriteLine($"Properity ID: {property.Id}, Terms: {model.Terms}");
                // Adding Terms and Conditions
                var termsAndConditions = new TermsAndConditions
                {
                    ProperityId = property.Id,
                    Terms = model.Terms,
                    PaymentTerms = model.PaymentTerms,
                    PenaltyClauses = model.PenaltyClauses,
                    MaintenanceResponsibility = model.MaintenanceResponsibility,
                    CancellationPolicy = model.CancellationPolicy,
                    RenewalPolicy = model.RenewalPolicy,
                    InsuranceRequirements = model.InsuranceRequirements
                };

                _context.TermsAndConditions.Add(termsAndConditions);
                _context.SaveChanges();


                ViewBag.Message = "Form successfully submitted!";
            }
            else
            {
                ViewBag.Message = "There was an error with the form.";
            }
            ViewBag.Features = _context.Features.ToList(); // Ensure the features are available in the view
            return RedirectToAction("Index", "MyProperity"); // Pass the model back to the view in case of validation errors
        }
    }
}
