using Microsoft.AspNetCore.Mvc;
using RealStats.Data;
using RealStats.Models;
using System.Linq;

namespace RealStats.Controllers
{
    public class AgreementContractController : Controller
    {
        private readonly RealStateContext _context;

        public AgreementContractController(RealStateContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Accept(int LeaseAgreementId)
        {
            var leaseAgreement = _context.LeaseAgreement.Find(LeaseAgreementId);
            if (leaseAgreement == null)
            {
                return NotFound();
            }

            UpdateLeaseAgreementStatus(leaseAgreement, 3); // 3 means accepted

            NotifyOtherTenants(leaseAgreement.ProperityId, LeaseAgreementId, "rejected");

            NotifyTenant(leaseAgreement.TenantId, "accepted" , leaseAgreement.ProperityId , leaseAgreement.Id);

            DeleteInboxMessagesByLeaseAgreementId(LeaseAgreementId);

            DeleteOtherLeaseAgreements(leaseAgreement.ProperityId, LeaseAgreementId);

            AddTenantToProperty(leaseAgreement.TenantId, leaseAgreement.ProperityId);

            AddPayments(leaseAgreement, leaseAgreement.LeaseDuration);

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Your action has been successfully sent.";

            return RedirectToAction("Index", "InboxManager");
        }
        [HttpPost]
        public IActionResult Reject(int LeaseAgreementId)
        {
            var leaseAgreement = _context.LeaseAgreement.Find(LeaseAgreementId);
            if (leaseAgreement == null)
            {
                return NotFound();
            }

            NotifyTenant(leaseAgreement.TenantId, "rejected", leaseAgreement.ProperityId, null);
            _context.LeaseAgreement.Remove(leaseAgreement);

            var inboxMessage = _context.InboxTenant
                .FirstOrDefault(i => i.LeaseAgreementId == LeaseAgreementId && i.TenantId == leaseAgreement.TenantId);
            if (inboxMessage != null)
            {
                _context.InboxTenant.Remove(inboxMessage);
            }
            _context.SaveChanges();
            TempData["Message"] = "Your action has been sent. The lease agreement has been rejected.";
            return RedirectToAction("Index", "InboxManager");
        }

        private void UpdateLeaseAgreementStatus(LeaseAgreement leaseAgreement, int status)
        {
            leaseAgreement.LeaseStatus = status;
            leaseAgreement.StartDate = DateTime.Now;
            leaseAgreement.EndDate = leaseAgreement.StartDate.AddMonths(leaseAgreement.LeaseDuration);
            var property = _context.Properities.Find(leaseAgreement.ProperityId);
            if (property != null)
            {
                property.Status = false;
                _context.Properities.Update(property);
            }
            _context.LeaseAgreement.Update(leaseAgreement); 
        }


        private void DeleteOtherLeaseAgreements(int propertyId, int currentLeaseAgreementId)
        {
            var otherAgreements = _context.LeaseAgreement
                .Where(l => l.ProperityId == propertyId && l.Id != currentLeaseAgreementId)
                .ToList();

            foreach (var agreement in otherAgreements)
            {
                _context.LeaseAgreement.Remove(agreement);
            }
        }

        private void NotifyTenant(int tenantId, string status, int propertyId, int? leaseAgreementId)
        {
            var tenantInbox = new InboxTenant
            {
                TenantId = tenantId,
                Description = $"Your lease request has been {status}.",
                Type = status == "accepted" ? "Accepted" : "Rejected",
                PropertyId = propertyId,
                LeaseAgreementId = (status == "rejected") ? (int?)null : leaseAgreementId
            };
            _context.InboxTenant.Add(tenantInbox);
        }

        private void NotifyOtherTenants(int propertyId, int acceptedLeaseAgreementId, string status)
        {
            var otherLeaseAgreements = _context.LeaseAgreement
                .Where(l => l.ProperityId == propertyId && l.Id != acceptedLeaseAgreementId)
                .ToList();

            foreach (var leaseAgreement in otherLeaseAgreements)
            {
                NotifyTenant(leaseAgreement.TenantId, status, propertyId, null); ;
            }
        }

        private void AddTenantToProperty(int tenantId, int propertyId)
        {
            var property = _context.Properities.Find(propertyId);
            if (property != null)
            {
                property.TenantId = tenantId;
                _context.Properities.Update(property);
            }
        }

        private void AddPayments(LeaseAgreement leaseAgreement, int leaseDuration)
        {
            var property = _context.Properities.Find(leaseAgreement.ProperityId);
            if (property == null)
            {
                return;
            }

            DateTime startDate = DateTime.Now;

            for (int i = 0; i < leaseDuration; i++)
            {
                var payment = new Payment
                {
                    LeaseAgreementId = leaseAgreement.Id,
                    Amount = property.Price,
                    IsPaid = false,
                    StartDate = startDate.AddMonths(i),
                    PaymentDate = startDate.AddMonths(i) 
                };

                _context.Payment.Add(payment);
            }
        }

        private void DeleteInboxMessagesByLeaseAgreementId(int leaseAgreementId)
        {
            var inboxMessages = _context.InboxManagers
                .Where(im => im.LeaseAgreementId == leaseAgreementId)
                .ToList();

            foreach (var message in inboxMessages)
            {
                _context.InboxManagers.Remove(message);
            }
            _context.SaveChanges();
        }




    }
}
