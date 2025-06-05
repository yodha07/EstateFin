using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        Booking GetById(int id);
        IEnumerable<Booking> GetAll();
        void Update(Booking booking);
        void Delete(int id);
        List<Booking> GetByUserId(int userId);
    }
}
