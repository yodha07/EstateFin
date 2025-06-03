using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EstateFin.Services
{
    public class BookingService : IBookingRepository
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment env;

        public BookingService(ApplicationDbContext db, IWebHostEnvironment env)
        { this.db = db; this.env = env; }
        public void Add(Booking booking)
        {
            db.Bookings.Add(booking);
            db.SaveChanges();
        }

        public Booking GetById(int id)
        {
            return db.Bookings
                .Include(b => b.Property)
                .Include(b => b.User)
                .FirstOrDefault(b => b.BookingId == id);
        }

        public IEnumerable<Booking> GetAll()
        {
            return db.Bookings.Include(b => b.Property).Include(b => b.User).Where(b => b.Status==0).ToList();
        }

        public void Update(Booking booking)
        {
            db.Bookings.Update(booking);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                db.Bookings.Remove(booking);
                db.SaveChanges();
            }
        }

        public List<Booking> GetByUserId(int userId)
        {
            return db.Bookings
                .Include(b => b.Property)
                .Include(b => b.User)
                .Where(b => b.UserID == userId)
                .ToList();
        }
    }
}
