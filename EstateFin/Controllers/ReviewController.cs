﻿using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;
using EstateFin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EstateFin.Controllers
{
    public class ReviewController : Controller
    {
        ApplicationDbContext db;
        ReviewRepo repo;

        public ReviewController(ReviewRepo repo, ApplicationDbContext db)
        {
            this.db = db;
            this.repo = repo;
        }

        public IActionResult Index(int propertyId)
        {
            var review = new Review
            {
                PropertyId = propertyId
            };

            return View(review);
        }

        //public IActionResult Submit(int propertyId)
        //{
        //    //ViewBag.PropertyId = propertyId;
        //    var model = new Review
        //    {
        //        PropertyId = 1
        //    };
        //    return View(model);
        //}

        public IActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(Review review, int id)
        {
            //review.UserID = 1;         // hardcoded user
            //review.UserID = int.Parse(HttpContext.Session.GetString("Login") ?? "0");
            review.PropertyId = id;     // hardcoded property
            //review.DatePosted = DateTime.Now;
            review.Id = 0;            
            //if (ModelState.IsValid)
            //{
            //    db.Reviews.Add(review);
            //    db.SaveChanges();

            //    return RedirectToAction("ViewPropertyReviews", new { id = review.PropertyId });
            //}
            repo.Submit(review);
            return RedirectToAction("ViewPropertyReviews", new { id = review.PropertyId });
        }

        //public IActionResult ViewPropertyReviews(int id)
        //{
        //    var reviews = db.Reviews.Where(r => r.PropertyId == id).ToList();
        //    return View(reviews);
        //}

        public IActionResult ViewPropertyReviews(int id)
        {
            var reviews = repo.GetReviewsByPropertyId(id);
            return View(reviews);
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalReviews = repo.GetTotalReviews();
            ViewBag.AvgRating = repo.GetAverageRating();

            return View();
        }
    }
}
