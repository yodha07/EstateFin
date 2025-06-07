using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface ReviewRepo
    {
        void Submit(Review review);

        List<Review> GetReviewsByPropertyId(int propertyId);

        int GetTotalReviews();
        double GetAverageRating();
    }
}
