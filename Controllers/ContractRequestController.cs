using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealStats.Data;
using RealStats.Models;
using System;
using System.IO;
using System.Threading.Tasks;

public class ContractRequestController : Controller
{
    private readonly RealStateContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ContractRequestController(RealStateContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost]
    public async Task<IActionResult> UploadSignedContract(IFormFile signedContract, int contractId, int managerId)
    {
        if (signedContract == null || signedContract.Length == 0)
        {
            ModelState.AddModelError("", "Please upload a valid file.");
            return View();
        }

        // Generate a unique file name to avoid conflicts
        var fileName = Path.GetFileNameWithoutExtension(signedContract.FileName);
        var fileExtension = Path.GetExtension(signedContract.FileName);
        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/contracts");
        Directory.CreateDirectory(uploadsFolder);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await signedContract.CopyToAsync(fileStream);
        }

        var file = new ContractFile
        {
            FileName = uniqueFileName,
            FilePath = filePath,
            UploadedAt = DateTime.Now,
            LeaseAgreementId = contractId
        };
        _context.ContractFiles.Add(file);


        // Update LeaseAgreement status to "1" (Requested)
        var leaseAgreement = await _context.LeaseAgreement.FindAsync(contractId);
        if (leaseAgreement != null)
        {
            leaseAgreement.LeaseStatus = 1;  // 1 means requested
        }

        // Send the lease agreement to InboxManager
        var inboxManager = new InboxManager
        {
            ManagerId = managerId,
            LeaseAgreementId = contractId,
            Description = "New signed contract uploaded by tenant",
            Type = "Contract Upload"
        };
        _context.InboxManagers.Add(inboxManager);

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "The signed contract has been uploaded and the manager has been notified.";
        return RedirectToAction("Details", "Property",new { propertyId = leaseAgreement.ProperityId});

    }
}
