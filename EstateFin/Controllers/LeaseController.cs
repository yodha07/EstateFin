using Microsoft.AspNetCore.Mvc;
using EstateFin.Models;
using EstateFin.ILeaseRepo;
using Microsoft.AspNetCore.Mvc.Rendering;
using EstateFin.Data;
using Microsoft.EntityFrameworkCore;
using EstateFin.Migrations;

namespace EstateFin.ILeaseRepo
{
    public class LeaseController : Controller
    {
        private readonly ILeaseRepo leaseRepo;
        private readonly ApplicationDbContext context;
        private static List<string> Combinedlist = new List<string>();
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

            int login = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            //var list = context.Bookings.Where(x => x.UserID.Equals(login) && x.Status.Equals(1)).Include(x => x.User).Include(x => x.Property).ToList();
            //int leaseStatusValue = (int)ILeaseRepo.LeaseStatus.Active;

            var list = context.Bookings.Include(x => x.Property).Include(x => x.User).ToList();


            //ViewBag.Properties = new SelectList(list, "PropertyId", "PropertyName");
            //ViewBag.Tenants = new SelectList(list, "UserID", "UserName");
            ViewBag.Bookings = new SelectList(list, "BookingId", "BookingId");


            var list = context.Bookings.Include(x => x.User).Include(x => x.Property);
            //var list = context.Bookings.Include(x=> x.User).Include(x=> x.)

            //int leaseStatusValue = (int)ILeaseRepo.LeaseStatus.Active;

            //ViewBag.Properties = new SelectList(context.Properties.ToList(), "PropertyId", "Title");
            //ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName");
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "BookingId");

            return View();
        }

        //[HttpPost]
        //public IActionResult Create(string propertyname, string Bookedby)
        //{
        //    string combinesd = $"{propertyname} booked by {Bookedby}";
        //    Combinedlist.Add(combinesd);
        //    return RedirectToAction(nameof(Index));

        //}

        // POST: Lease/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(LeaseAgreement lease)
        {

            int id = lease.BookingId;
            var list = context.Bookings.Find(id);

            lease.PropertyId = list.PropertyId;
            lease.TenantId = list.UserID;



            var userId = int.Parse(HttpContext.Session.GetString("Login") ?? "0");
            lease.TenantId = userId; 
            var id = context.Bookings.Find(lease.BookingId);
            lease.PropertyId = id.PropertyId;

            if (ModelState.IsValid)
            {
                leaseRepo.Add(lease);
                leaseRepo.Save();
                return RedirectToAction(nameof(Index));
            }

            //var list = context.Bookings.Include(x => x.Property).Include(x => x.User).ToList();



            ViewBag.Properties = new SelectList(context.Properties.ToList(), "PropertyId", "PropertyName", lease.PropertyId);
            ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName", lease.TenantId);
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "DisplayName", lease.Booking);
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

            var e = context.Properties.Where(x => x.PropertyId.Equals(lease.PropertyId)).ToList();

            ViewBag.Properties = new SelectList(e, "PropertyId", "PropertyName");
            ViewBag.Tenants = new SelectList(context.Users.ToList(), "UserID", "UserName", lease.TenantId);
            ViewBag.Bookings = new SelectList(context.Bookings.ToList(), "BookingId", "BookingId", lease.BookingId);

            return View(lease);
        }

        // POST: Lease/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LeaseAgreement lease)
        {
            //Created 


            int id = lease.BookingId;
            var list = context.Bookings.Find(id);

            lease.PropertyId = list.PropertyId;
            lease.TenantId = list.UserID;

            if (ModelState.IsValid)
            {
                leaseRepo.Update(lease);
                leaseRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            //var list = context.Bookings.Include(x => x.Property).Include(x => x.User).ToList();

            //ViewBag.Properties = new SelectList(list, "PropertyId", "PropertyName");
            //ViewBag.Tenants = new SelectList(list, "UserID", "UserName");
            //ViewBag.Bookings = new SelectList(list, "BookingId", "BookingId");
            return View(lease);
        }

        // GET: Lease/Delete/5
        public IActionResult Delete(int id)
        {
            //var lease = leaseRepo.GetById(id);
            var lease = leaseRepo.getId(id) ;

            if (lease == null)
            {
                return NotFound();
            }
            leaseRepo.Del(id);
            return RedirectToAction("Index");

            //return View(lease);
        }

        // POST: Lease/Delete/5

        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{

        //    leaseRepo.Delete(id);
        //    leaseRepo.Save();
        //    return RedirectToAction("Index");
        //}

        // GET: Lease/Details/5
        //public IActionResult Details(int id)
        //{
        //    var lease = leaseRepo.GetById(id);
        //    if (lease == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(lease);
        //}
    }
}


