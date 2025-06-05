using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Models.Enum.StatusEnums;
using EstateFin.Repositories;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace EstateFin.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository transactionService;
        private readonly IBookingRepository _bookingService;
        private readonly IUserRepo repo;
        private readonly ApplicationDbContext db;
        public TransactionController(ITransactionRepository transactionService, IBookingRepository bookingService, ApplicationDbContext db, IUserRepo repo)
        {
            this.repo = repo;
            this.transactionService = transactionService;
            _bookingService = bookingService;
            this.db = db;
        }

        public IActionResult Index()
        {
            var userID = int.Parse(HttpContext.Session.GetString("Login") ?? "0");
            var transactions = transactionService.GetAll(userID);
            return View(transactions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Transaction transaction)
        {
            transactionService.Add(transaction);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            transactionService.Delete(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult PaymentSuccess(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, double amount, int bookingId)
        {
            Transaction txn = new Transaction
            {
                BookingId = bookingId,
                PaymentId = razorpay_payment_id,
                PaymentMethod = "Razorpay",
                PaymentStatus = PaymentStatus.Success,
                TransactionDate = DateTime.Now,
                Amount = (decimal)amount
            };

            transactionService.Add(txn);
            TempData["razorpay_order"] = razorpay_payment_id;
            var booking = _bookingService.GetById(bookingId);
            if (booking != null)
            {
                booking.Status = EstateFin.Models.Enum.StatusEnums.BookingStatus.Confirmed;
                _bookingService.Update(booking);
            }
            int id = booking.PropertyId;
            var pname = db.Properties.Find(id).Title;
            var email = booking.User.Email;
            var confirm = db.Properties.Find(id);
            confirm.Status = "Sold";
            db.SaveChanges();
            //add pdf gen and sendmail here
            repo.SendEmailAsync(email,
                "Payment successful",
                $"Your payment for property: {pname} is successful\n" +
                $"Your payment id : {razorpay_payment_id}\n" +
                $"Your payment amount: {amount}\n" +
                $"Transaction Date: {DateTime.Now}").Wait();
            return RedirectToAction("DownloadReceipt");

        }
        public IActionResult MakePayment(int id)
        {
            var ids = db.Bookings.Find(id);
            ViewBag.BookingId = ids.BookingId;
            ViewBag.Amount = decimal.Parse(ids.Amount.ToString());
            return View();
        }

        public IActionResult DownloadReceipt()
        {
            string id = TempData["razorpay_order"].ToString();
            var list = db.Transactions.Where(x => x.PaymentId.Equals(id)).ToList();


            return View(list);
        }

    }
}

