using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;

namespace EstateFin.Services
{
    public class TransactionService : ITransactionRepository
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment env;

        public TransactionService(ApplicationDbContext db, IWebHostEnvironment env)
        { this.db = db; this.env = env; }

        public void Add(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        public Transaction GetById(int id)
        {
            return db.Transactions
                     .Where(t => t.TransactionId == id)
                     .FirstOrDefault();
        }

        public IEnumerable<Transaction> GetAll(int id)
        {
            return db.Transactions
                     .Where(t => t.BookingId == id)
                     .ToList();
        }

        public void Update(Transaction transaction)
        {
            db.Transactions.Update(transaction);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var transaction = db.Transactions.Find(id);
            if (transaction != null)
            {
                db.Transactions.Remove(transaction);
                db.SaveChanges();
            }
        }
    }
}
