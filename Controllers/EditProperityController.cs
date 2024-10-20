using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;
using System.Security.Claims;

namespace RealStats.Controllers
{
    public class EditProperityController : Controller
    {
        private readonly RealStateContext _context;
        public EditProperityController(RealStateContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize, Route("EditProperity/Edit/{ProperityId:int}")]
        public IActionResult Edit(int ProperityId)
        {
            var property = _context.Properities.FirstOrDefault(p => p.Id == ProperityId);
            var terms = _context.TermsAndConditions.FirstOrDefault(t => t.ProperityId == ProperityId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _context.Managers.FirstOrDefault(m => m.UserId == userId);

            if (property == null)
            {
                return NotFound();
            }
            var selectProperity = _context.Properities.Include(p => p.Features).FirstOrDefault(p => p.Id == ProperityId);
            var selectedFeatures = selectProperity?.Features.Select(p => p.Id).ToList();
            if (property.ManagerId != manager?.Id)
            {
                return Unauthorized(manager);
            }

            var model = new AddProperityModel()
            {
                PropertyId = property.Id,
                Name = property.Name,
                Description = property.Description,
                Price = property.Price,
                City = property.City,
                Country = property.Country,
                Street = property.Street,
                Bathrooms = property.Bathrooms,
                Bedrooms = property.Bedrooms,
                Garages = property.Garages,
                Area = property.Area,
                Features = selectedFeatures,
                Terms = terms.Terms,
                PaymentTerms = terms.PaymentTerms,
                PenaltyClauses = terms.PenaltyClauses,
                MaintenanceResponsibility = terms.MaintenanceResponsibility,
                CancellationPolicy = terms.CancellationPolicy,
                RenewalPolicy = terms.RenewalPolicy,
                InsuranceRequirements = terms.InsuranceRequirements
            };

            ViewBag.Features = _context.Features.ToList();
            return View("~/Views/AddProperity/Index.cshtml", model);
        }

        [HttpPost]
        [Authorize, Route("EditProperity/Edit/{ProperityId:int}")]
        public IActionResult Edit(int ProperityId, AddProperityModel model)
        {
            var property = _context.Properities
                .Include(p => p.Features)
                .Include(i => i.Images)
                .FirstOrDefault(p => p.Id == ProperityId);

            var terms = _context.TermsAndConditions
                .FirstOrDefault(t => t.ProperityId == ProperityId);

            if (property == null)
            {
                return NotFound();
            }
            property.Features.Clear();
            property.Images.Clear();


            if (model.Features != null && model.Features.Count > 0)
            {
                var selectedFeatures = _context.Features.Where(f => model.Features.Contains(f.Id)).ToList();
                property.Features.AddRange(selectedFeatures);
            }

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


            property.Name = model.Name;
            property.Description = model.Description;
            property.Price = model.Price;
            property.City = model.City;
            property.Country = model.Country;
            property.Street = model.Street;
            property.Bathrooms = model.Bathrooms;
            property.Bedrooms = model.Bedrooms;
            property.Garages = model.Garages;
            property.Area = model.Area;
            if (terms != null)
            {
                terms.Terms = model.Terms;
                terms.PaymentTerms = model.PaymentTerms;
                terms.PenaltyClauses = model.PenaltyClauses;
                terms.MaintenanceResponsibility = model.MaintenanceResponsibility;
                terms.CancellationPolicy = model.CancellationPolicy;
                terms.RenewalPolicy = model.RenewalPolicy;
                terms.InsuranceRequirements = model.InsuranceRequirements;
            }
            _context.SaveChanges();

            return Redirect($"/Properity/{ProperityId}"); ;
        }

        [HttpPost]
        [Authorize, Route("EditProperity/Delete/{propertyId:int}")]
        public IActionResult Delete(int propertyId)
        {
            var property = _context.Properities
            .Include(p => p.TermsAndConditions)
            .Include(p => p.LeaseAgreements)
            .Include(p => p.Tenants)
            .Include(p => p.ReportIssues)
            .Include(p => p.Features)
            .Include(p => p.Images)
            .FirstOrDefault(p => p.Id == propertyId);

            if (property == null)
            {
                return NotFound();
            }

            property.Features.Clear();

            _context.Properities.Remove(property);
            _context.SaveChanges();

            return RedirectToAction("Index", "MyProperity");
        }
    }


}
