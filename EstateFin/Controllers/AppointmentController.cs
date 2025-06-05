using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EstateFin.Controllers
{
    public class AppointmentController : Controller
    {
        ApplicationDbContext db;
        private readonly IAppointmentRepo repo;
        public AppointmentController(ApplicationDbContext db, IAppointmentRepo repo) 
        {
            this.db = db;
            this.repo = repo;
        }
        

        public IActionResult Index()
        {
            int user = int.Parse(HttpContext.Session.GetString("Login"));
            //int propertyid = int.Parse(HttpContext.Session.GetString("propId"));


            var e =  db.appointment.Where(x=> x.UserID.Equals(user) ).Include(x=> x.Property).Include(x=> x.User).ToList();
            return View(e);
        }

        //[Authorize(Roles = "Agent, Seller")]
        //public IActionResult agent()
        //{
        //    int user = int.Parse(HttpContext.Session.GetString("Login"));
        //    int propertyId = int.Parse(HttpContext.Session.GetString("propId") ?? "0");

        //    var appointments = db.appointment
        //        .Where(x => x.PropertyId == propertyId && x.Status.Equals("pending"))
        //        .Include(x => x.Property)
        //        .Include(x => x.User)
        //        .ToList();

        //    return View(appointments);
        //}


        //[Authorize(Roles = "Agent, Seller")]
        //public IActionResult agent()
        //{
        //    int user = int.Parse(HttpContext.Session.GetString("Login"));

        //    // Get the property that belongs to the logged-in agent/seller
        //    var property = db.Properties.FirstOrDefault(p => p.UserID == user);
        //    if (property == null)
        //    {
        //        // Handle the case where no property is found for the user.
        //        return View(new List<Appointment>());
        //    }

        //    int propertyId = property.PropertyId;

        //    var appointments = db.appointment
        //        .Where(x => x.PropertyId == propertyId && x.Status.Equals("pending"))
        //        .Include(x => x.Property)
        //        .Include(x => x.User)
        //        .ToList();

        //    return View(appointments);
        //}

        [Authorize(Roles = "Agent, Seller")]
        public IActionResult agent()
        {
            int user = int.Parse(HttpContext.Session.GetString("Login"));
            var propertyIds = repo.PropertyIdNull(user);

            if (!propertyIds.Any())
            {
                return View(new List<Appointment>());
            }

            var appointments = db.appointment
                .Where(x => propertyIds.Contains(x.PropertyId) && x.Status.Equals("pending"))
                .Include(x => x.Property)
                .Include(x => x.User)
                .ToList();

            return View(appointments);
        }



        [Authorize(Roles = "Agent, Seller")]
        public IActionResult confirm(int id)
        {
            var user = repo.GetAppointment(id);
            user.Status = "confirmed";
            repo.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize(Roles = "Agent, Seller")]
        public IActionResult reject(int id)
        {

            var user = repo.GetAppointment(id);
            repo.DeleteChanges(user);
            repo.SaveChanges();
            return RedirectToAction("index");
        }

        [Authorize(Roles = "Buyer, Tenant")]
        public IActionResult Add_Appointment()
        {
            var list = repo.GetSlots();

            ViewBag.slot = new SelectList(list, "Slot_type", "Slot_type");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Buyer, Tenant")]
        public IActionResult Add_Appointment(int id , Appointment app)
        {
            int user = int.Parse(HttpContext.Session.GetString("Login"));

            var e = repo.GetUser(id);
            app.UserID = user;
            app.Status = "pending";
            app.PropertyId = id;
            //foreach (var modelError in ModelState)
            //{
            //    foreach (var error in modelError.Value.Errors)
            //    {
            //        Console.WriteLine($"Field: {modelError.Key} — Error: {error.ErrorMessage}");
            //    }
            //}
            //var app = new Appointment()
            //{
            //    UserID = user,
            //};
            //app.UserID = user;
            //app.Status = "pending";
            //app.PropertyId = id;
            if (ModelState.IsValid) 
            {
                if (db.appointment.Any(a => a.AppointmentDate == app.AppointmentDate && a.slot == app.slot && a.PropertyId == app.PropertyId && a.Status == "pending"))
                {
                    ViewBag.msg = "Slot is already Booked";
                    var list = db.slot.Where(x => x.Status.Equals("Active")).ToList();
                    
                    ViewBag.slot = new SelectList(list, "Slot_type", "Slot_type");

                    return View();   
                }
                else
                {
                    HttpContext.Session.SetString("propId", app.PropertyId.ToString()!);

                    db.appointment.Add(app);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
            }
            else
            {
                return View();   
            }
        }

        [Authorize(Roles = "Agent, Seller")]
        public IActionResult slot_add()
        {

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Agent, Seller")]
        public IActionResult slot_add(Slot slots)
        {
            if (ModelState.IsValid)
            {
                db.slot.Add(slots);
                db.SaveChanges();
                ViewBag.slotadded = "slot added";
            }
            return View();
        }

    }
}
