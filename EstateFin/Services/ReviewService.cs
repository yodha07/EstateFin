using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EstateFin.Services
{
    public class ReviewService : ReviewRepo
    {
        ApplicationDbContext db;

        public ReviewService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Submit(Review review)
        {
            //var userIdString = HttpContext.Session.GetString("Login");
            //if (string.IsNullOrEmpty(userIdString))
            //{
            //    // User is not logged in - handle this scenario
            //    throw new Exception("User is not logged in.");
            //}

            //int userId = int.Parse(userIdString);

            //review.UserID = userId;
            review.UserID = 1;         // hardcoded user
            review.PropertyId = 1;     // hardcoded property
            review.DatePosted = DateTime.Now;

            db.Reviews.Add(review);
            db.SaveChanges();
  
        }

        public List<Review> GetReviewsByPropertyId(int propertyId)
        {
             var data=db.Reviews.Where(r => r.PropertyId == propertyId).ToList();
             return data;
        }

        public int GetTotalReviews()
        {
            return db.Reviews.Count();
        }

        public double GetAverageRating()
        {
            return db.Reviews.Any() ? db.Reviews.Average(r => r.Rating) : 0;
        }
    }
}
