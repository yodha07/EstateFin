using EstateFin.Models.Enum.StatusEnums;
using EstateFin.Models;
using Microsoft.AspNetCore.Mvc;
using EstateFin.Repositories;
using EstateFin.Data;

namespace EstateFin.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingService;
        private readonly IUserRepo repo;
        private readonly ApplicationDbContext db;
        public BookingController(IBookingRepository bookingService, ApplicationDbContext db)
        {
            this.db = db;
            _bookingService = bookingService;
        }

        // GET: Booking Create View (if needed)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Booking - Main flow for user booking with Razorpay
        [HttpPost]
        public IActionResult Create(int PropertyId, decimal Amount)
        {
            int userId = int.Parse((HttpContext.Session.GetString("Login") ?? "0"));
            Booking booking = new Booking
            {
                PropertyId = PropertyId,
                UserID = userId,
                BookingDate = DateTime.Parse(TempData["date"].ToString()),
                Amount = Amount,
                Status = BookingStatus.Pending
            };

            _bookingService.Add(booking);

            // Save booking ID in session
            HttpContext.Session.SetString("BookingId", booking.BookingId.ToString());

            // Redirect to Razorpay Payment
            return RedirectToAction("MakePayment", "Transaction", new { bookingId = booking.BookingId, amount = Amount });
        }

        // GET: All Bookings (Admin)
        public IActionResult Index()
        {
            var bookings = _bookingService.GetAll();
            return View(bookings);
        }

        // GET: My Bookings (User-specific)
        public IActionResult MyBookings()
        {
            int userId = int.Parse(HttpContext.Session.GetString("Login") ?? "0");
            var bookings = _bookingService.GetByUserId(userId);
            
            return View(bookings);
        }

        // POST: Approve booking by admin
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var booking = _bookingService.GetById(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Approved;
                _bookingService.Update(booking);
            }
            //repo.SendEmailAsync().Wait();                  
            return RedirectToAction("Index");
            //// Admin check
            //if (HttpContext.Session.GetString("UserRole") != "Admin")
            //    return Unauthorized();
               
        }

        // POST: Reject booking by admin
        //[HttpPost]
        //public IActionResult Reject(int id)
        //{
        //    var booking = _bookingService.GetById(id);
        //    if (booking != null)
        //    {
        //        booking.Status = BookingStatus.Rejected;
        //        _bookingService.Update(booking);
        //    }

        //    return RedirectToAction("Index");
        //    //// Admin check
        //    //if (HttpContext.Session.GetString("UserRole") != "Admin")
        //    //    return Unauthorized();


        //}

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var booking = _bookingService.GetById(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Rejected;
                _bookingService.Update(booking);

                var property = db.Properties.Find(booking.PropertyId);
                if (property != null)
                {
                    property.Status = "Available";
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

    }
}
