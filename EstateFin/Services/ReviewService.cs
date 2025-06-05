using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EstateFin.Services
{
    public class ReviewService : ReviewRepo
    {
        ApplicationDbContext db;
        private readonly IHttpContextAccessor co;

        public ReviewService(ApplicationDbContext db, IHttpContextAccessor co)
        {
            this.db = db;
            this.co = co;
        }

        public void Submit(Review review)
        {
            var userIdString = co.HttpContext?.Session?.GetString("Login");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                throw new Exception("User is not logged in or session is invalid.");
            }

            // Set values
            review.UserID = userId;

            if (review.PropertyId <= 0)
            {
                throw new Exception("Invalid Property ID.");
            }

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
