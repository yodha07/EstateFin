using Microsoft.AspNetCore.Mvc;
using EstateFin.Models;
using EstateFin.ILeaseRepo;
using Microsoft.AspNetCore.Mvc.Rendering;
using EstateFin.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateFin.ILeaseRepo
{
    public class LeaseController : Controller
    {
        private readonly ILeaseRepo leaseRepo;
        private readonly ApplicationDbContext context;

        public LeaseController(ILeaseRepo leaseRepo, ApplicationDbContext context)
        {
            this.leaseRepo = leaseRepo;
            this.context = context;
        }

        // GET: Lease
        public IActionResult Index()
        {
            var leases = leaseRepo.GetAll();
            return View(leases);
        }

        // GET: Lease/Create
        public IActionResult Create()

        {
            var list = context.Bookings.Include(x => x.User).Include(x => x.Property);
            //int leaseStatusValue = (int)ILeaseRepo.LeaseStatus.Active;

            ViewBag.Properties = new SelectList(context.Properties.ToList(), "PropertyId", "Title");
            ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName");
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "BookingId");
            return View();
        }

        // POST: Lease/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(LeaseAgreement lease)
        {
            if (ModelState.IsValid)
            {
                leaseRepo.Add(lease);
                leaseRepo.Save();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Properties = new SelectList(context.Properties.ToList(), "PropertyId", "PropertyName", lease.PropertyId);
            ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName", lease.TenantId);
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "BookingId", lease.BookingId);
            return View(lease);
        }

        // GET: Lease/Edit/5
        public IActionResult Edit(int id)
        {
            var lease = leaseRepo.GetById(id);
            if (lease == null)
            {
                return NotFound();
            }

            ViewBag.Properties = new SelectList(context.Properties.ToList(), "PropertyId", "PropertyName", lease.PropertyId);
            ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName", lease.TenantId);
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "BookingId", lease.BookingId);

            return View(lease);
        }

        // POST: Lease/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LeaseAgreement lease)
        {
            if (ModelState.IsValid)
            {
                leaseRepo.Update(lease);
                leaseRepo.Save();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Properties = new SelectList(context.Properties.ToList(), "PropertyId", "PropertyName", lease.PropertyId);
            ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName", lease.TenantId);
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "BookingId", lease.BookingId);
            return View(lease);
        }

        // GET: Lease/Delete/5
        public IActionResult Delete(int id)
        {
            var lease = leaseRepo.GetById(id);
            if (lease == null)
            {
                return NotFound();
            }
            return View(lease);
        }

        // POST: Lease/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            leaseRepo.Delete(id);
            leaseRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Lease/Details/5
        public IActionResult Details(int id)
        {
            var lease = leaseRepo.GetById(id);
            if (lease == null)
            {
                return NotFound();
            }
            return View(lease);
        }
    }
}
