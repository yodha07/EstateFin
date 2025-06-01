using EstateFin.Data;
using EstateFin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateFin.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReviewController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            //var review = new Review
            //{
            //    UserId = 1,
            //    PropertyId = 99,
            //    Rating = 5,
            //    Comment = "Test insert",
            //    DatePosted = DateTime.Now
            //};
            //db.Reviews.Add(review);
            //db.SaveChanges();
            //return Content("Inserted!");
            return View();
        }

        public IActionResult Submit(int propertyId)
        {
            //ViewBag.PropertyId = propertyId;
            var model = new Review
            {
                PropertyId = propertyId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Submit(Review review)
        {
            if (ModelState.IsValid)
            {
                review.UserId = 1;  //hard coded
                review.DatePosted = DateTime.Now;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("ViewPropertyReviews", new { id = review.PropertyId });
            }

            return View(review);

        }

        public IActionResult ViewPropertyReviews(int id)
        {
            var reviews = db.Reviews.Where(r => r.PropertyId == id).ToList();
            return View(reviews);
        }
    }
}
