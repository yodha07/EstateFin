using EstateFin.Data;
using Microsoft.AspNetCore.Mvc;

namespace EstateFin.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReportController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var totalReviews = db.Reviews.Count();
            var avgRating = db.Reviews.Average(r => r.Rating);

            ViewBag.TotalReviews = totalReviews;
            ViewBag.AvgRating = avgRating;

            return View();
            //var reviews = db.Reviews.ToList();

            //ViewBag.TotalReviews = reviews.Count;
            //ViewBag.AvgRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 2) : 0;

            //// Example: Group by property
            //var grouped = reviews.GroupBy(r => r.Property.Name)
            //                     .Select(g => new {
            //                         PropertyName = g.Key,
            //                         AvgRating = Math.Round(g.Average(r => r.Rating), 2)
            //                     }).ToList();

            //ViewBag.PropertyNames = grouped.Select(g => g.PropertyName).ToList();
            //ViewBag.AvgRatings = grouped.Select(g => g.AvgRating).ToList();

            //return View();
        }
    }
}
