﻿using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Models.Enum.StatusEnums;
using EstateFin.Repositories;
using EstateFin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Razorpay.Api;
using System.Linq;

namespace EstateFin.Controllers
{
    public class PropertiesController : Controller
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly PropertyRepo repo;
        private readonly IBookingRepository bookingRepository;

        public PropertiesController(ApplicationDbContext db, IWebHostEnvironment env, PropertyRepo repo, IBookingRepository bookingRepository) { this.db = db; this.env = env; this.repo = repo; this.bookingRepository = bookingRepository; }
        public IActionResult Index(int id)
        {
            var userID = int.Parse(HttpContext.Session.GetString("Login")!);
            var list = repo.GetProperties(userID);

            return View(list);
        }


        [HttpPost]
        public IActionResult Index(string txt)
        {
            if (txt != null)
            {
                var list =repo.search(txt);

                return View(list);
            }
            else
            {
                var list =repo.FetchAllProperty();
                return View(list);
            }
        }



        public IActionResult add_properties()
        {
            //var my_propertiess = db.Property_Types.Where(x => x.status.Equals("Active")).Select(x => new SelectListItem
            //{
            //    Value = x.PropertyType.ToString(),
            //    Text = x.PropertyType.ToString()

            //}).ToList();
            //var dropdown = new dropdowns() 
            //{ 
            //    myproperties = my_propertiess
            //};
            //var propertiess = new properties() { };


            //var bind = new Bind
            //{
            //    properties = propertiess,

            //    dropdowns = dropdown
            //};
            ViewBag.myproperties = new SelectList(repo.dropdown(), "MyPropertyId", "PropertyType");
            return View();
        }

        //[Authorize(Roles = "Agent, Seller")]
        //[HttpPost]
        //public IActionResult add_properties(Bind prop)
        //{
        //    prop.properties.UserID = int.Parse(HttpContext.Session.GetString("Login"));

        //    var mpath = repo.propertyfile(prop);
        //    prop.properties.images = string.Join(",", mpath);
        //    prop.properties.CreatedAt = DateTime.Now;
        //    HttpContext.Session.SetInt32("PropertyID", prop.properties.PropertyId);

        //    //foreach (var modelError in ModelState)
        //    //{
        //    //    foreach (var error in modelError.Value.Errors)
        //    //    {
        //    //        Console.WriteLine($"Field: {modelError.Key} — Error: {error.ErrorMessage}");
        //    //    }
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        repo.add_property(prop);
        //        TempData["msg"] = "data added";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        public IActionResult add_properties(Bind prop)
        {
            
            prop.properties.UserID = int.Parse(HttpContext.Session.GetString("Login"));

            
            var mpath = repo.propertyfile(prop);
            prop.properties.images = string.Join(",", mpath);
            prop.properties.CreatedAt = DateTime.Now;
            HttpContext.Session.SetInt32("PropertyID", prop.properties.PropertyId);

            
            //foreach (var key in ModelState.Keys)
            //{
            //    var errors = ModelState[key].Errors;
            //    if (errors.Count > 0)
            //    {
            //        foreach (var error in errors)
            //        {
            //            Console.WriteLine($"Field: {key} - Error: {error.ErrorMessage}");
            //        }
            //    }
            //}

            if (ModelState.IsValid)
            {
                repo.add_property(prop);
                TempData["msg"] = "Property Added";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.myproperties = new SelectList(repo.dropdown(), "MyPropertyId", "PropertyType");
                return View(prop);
            }
        }

        //[Authorize(Roles = "Admin, Agent, Seller")]
        public IActionResult Delete_Properties(int id)
        {
            if (id != null)
            {
                repo.delete_properties(id);
                TempData["del"] = "deleted";
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //[Authorize(Roles = "Agent, Seller")]
        public IActionResult Edit_Properties(int id)
        {

            ViewBag.editproperties = new SelectList(repo.dropdown(), "MyPropertyId", "PropertyType");

            //var my_propertiess = db.Property_Types.Where(x => x.status.Equals("Active")).Select(x => new SelectListItem
            //{
            //    Text = x.PropertyType,
            //    Value = x.PropertyType
            //}).ToList();
            //var dropdown = new dropdowns()
            //{
            //    myproperties = my_propertiess ,

            //};
            //var propertiess = new properties() { } ;

            var bind = repo.edit_property(id);


            return View(bind);
        }


        [HttpPost]
        //[Authorize(Roles = "Agent, Seller")]
        public IActionResult Edit_Properties(Bind e)
        {
            e.properties.UserID = int.Parse(HttpContext.Session.GetString("Login"));

            var mpath = repo.propertyfile(e);

            e.properties.images = string.Join(",", mpath);
            e.properties.CreatedAt = DateTime.Now;
            //foreach (var modelError in ModelState)
            //{
            //    foreach (var error in modelError.Value.Errors)
            //    {
            //        Console.WriteLine($"Field: {modelError.Key} — Error: {error.ErrorMessage}");
            //    }
            //}

            if (ModelState.IsValid)
            {
                repo.edit_property_post(e);
                TempData["updt"] = "updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                //var my_propertiess = db.Property_Types.Where(x => x.status.Equals("Active")).Select(x => new SelectListItem { Text = x.PropertyType, Value = x.PropertyType }).ToList();
                //e.dropdowns = new dropdowns() { myproperties = my_propertiess };
                return View();
            }



        }

        [Authorize(Roles = "Admin")]
        public IActionResult property_type()
        {
            return View();

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult property_type(Property_Type e)
        {
            e.createdat = DateTime.Now;
            e.createdby = HttpContext.Session.GetString("UserName");
            ;

            if (ModelState.IsValid)
            {
                repo.property_typess(e);
                return RedirectToAction("property_type_list");
            }
            else
            {
                return View();
            }
        }
        public IActionResult property_type_list()
        {
            var e = repo.FetchAllPropertyType();

            return View(e);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult delete_property_type(int id)
        {
            repo.delete_property_typess(id);

            return RedirectToAction("property_type_list");

        }

        [Authorize(Roles = "Admin")]
        public IActionResult edit_property_type(int id)
        {
            var ids =repo.Edit_Property_Type(id); 
            return View(ids);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult edit_property_type(Property_Type e)
        {
            e.createdat = DateTime.Now;
            e.createdby = "Session";
            if (ModelState.IsValid)
            {
                repo.edit_property_typess(e);
                return RedirectToAction("property_type_list");

            }
            else
            {
                return View();
            }



        }

        [Authorize(Roles = "Admin, Buyer, Tenant, Seller, Tenant")]
        public IActionResult property_user()
        {
            var data = repo.Property_User_List();
            if(data.Count == 0)
            {
                TempData["listMsg"] = "No properties listed";
                return View();
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Buyer, Tenant, Seller, Tenant")]
        public IActionResult property_user(int id)
        {
            //HttpContext.Session.SetString("UserRole", id.ToString());
            var PropertyId = repo.property_user_findbyid(id);
            int userId = int.Parse((HttpContext.Session.GetString("Login") ?? "0"));
            Booking booking = new Booking
            {
                PropertyId = PropertyId.PropertyId,
                UserID = userId,
                BookingDate = DateTime.Now,
                Amount = PropertyId.Price,
                Status = BookingStatus.Pending
            };
            

            bookingRepository.Add(booking);
            return RedirectToAction("MyBookings", "Booking");
        }

        public IActionResult property_user_tenant()
        {
            var data = repo.Property_Tenant_List();
            return View(data);
        }

        [HttpPost]
        public IActionResult property_user_tenant(int id)
        {
            //HttpContext.Session.SetString("UserRole", id.ToString());
            var PropertyId =repo.property_user_findbyid(id);
            int userId = int.Parse((HttpContext.Session.GetString("Login") ?? "0"));
            Booking booking = new Booking
            {
                PropertyId = PropertyId.PropertyId,
                UserID = userId,
                BookingDate = DateTime.Now,
                Amount = PropertyId.Price,
                Status = BookingStatus.Pending
            };

            bookingRepository.Add(booking);
            return RedirectToAction("MyBookings", "Booking");
        }

        public IActionResult Index_tenant()
        {
            int userId = int.Parse((HttpContext.Session.GetString("Login") ?? "0"));

            var list = repo.GetProperties(userId);

            return View(list);
        }


        [HttpPost]
        public IActionResult Index_tenant(string txt)
        {
            if (txt != null)
            {
                var list = repo.search(txt);

                return View(list);
            }
            else
            {
                var list = repo.FetchAllProperty();
                return View(list);
            }
        }


        [Authorize(Roles = "Buyer, Tenant")]
        public IActionResult property_user_review()
        {
            var data = repo.FetchAllProperty();
            if (data.Count == 0)
            {
                TempData["listMsg"] = "No properties listed";
                return View();
            }
            return View(data);
        }

    }

}
