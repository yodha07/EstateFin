using EstateFin.Repositories;
using Microsoft.AspNetCore.Mvc;
using EstateFin.Models.Enum.StatusEnums;


namespace EstateFin.Controllers
{

    public class TenantLeaseController : Controller
    {
        ILeaseTenantRepo ltrepo;
        public TenantLeaseController(ILeaseTenantRepo ltrepo)
        {
            this.ltrepo = ltrepo;
        }
        public IActionResult Index()
        {
            int tenantId = 1;
            var leases = ltrepo.GetLeasesByTenantId(tenantId);
            return View(leases);
        }

        public IActionResult LeaseDetails(int id)
        {
            var lease = ltrepo.GetLeaseById(id);
            return View(lease);
        }

        [HttpPost]
        public IActionResult AcceptAgreement(int id)
        {
            ltrepo.AcceptLeaseAgreement(id);
            TempData["msg"] = "Lease Accepted!";
            return RedirectToAction("LeaseDetails", new { id });
        }

        [HttpPost]
        public IActionResult RejectAgreement(int id)
        {
            ltrepo.RejectLeaseAgreement(id);
            TempData["msg"] = "Lease Rejected!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult PayDeposit(int id)
        {
            ltrepo.PaySecurityDeposit(id);
            TempData["msg"] = "Security deposit paid!";
            return RedirectToAction("LeaseDetails", new { id });
        }

        public IActionResult RentList(int leaseId)
        {
            var rents = ltrepo.GetRentByLeaseId(leaseId);
            ViewBag.LeaseId = leaseId;
            return View(rents);
        }

        public IActionResult PayRent(int id)
        {
            var rent = ltrepo.GetRentById(id);
            if (rent != null)
            {
                ltrepo.PayRent(id);
                TempData["msg"] = "Rent Paid!";
                return RedirectToAction("RentList", new { leaseId = rent.LeaseId });
            }
            return NotFound();
        }
    }
}

