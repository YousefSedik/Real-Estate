using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealStats.Data;
using RealStats.Models;
using RealStats.ViewModel;

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
            return View();
        }


        [HttpPost]
        public IActionResult Index(AddProperityModel model)
        {
            if (ModelState.IsValid)
            {
                Properity properity = new Properity()
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
                    Status = true
                };

                List<Feature> feature = new List<Feature>();
                if (model.Features != null && model.Features.Count > 0)
                {
                    feature = _context.Features.Where(f => model.Features.Contains(f.Id)).ToList();
                    properity.Features = feature;
                }
                if (model.images != null && model.images.Length > 0)
                {
                    foreach (var image in model.images)
                    {
                        if (image.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                image.CopyTo(stream);
                            }
                            var img = new Image
                            {
                                ImageUrl = "/images/" + uniqueFileName,
                                Properity = properity
                            };
                            if (properity.Images == null)
                            {
                                properity.Images = new List<Image>();
                            }
                            properity.Images.Add(img);
                        }
                    }
                }
                _context.Properities.Add(properity);
                _context.SaveChanges();
                ViewBag.Message = "Form successfully submitted!";
            }
            else
            {
                ViewBag.Message = "There was an error with the form.";
            }
            return View();
        }
    }
}