using EstateFin.Data;
using EstateFin.Models.Enum.StatusEnums;
using EstateFin.Models;
using EstateFin.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EstatePro.Services
{
    public class LeaseTenantService : ILeaseTenantRepo
    {
        ApplicationDbContext db;
        public LeaseTenantService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<LeaseAgreement> GetLeasesByTenantId(int id)
        {
            var data = db.LeaseAgreements
                     .Include(l => l.Property)
                     .Include(l => l.Booking)
                     .Where(l => l.TenantId == id)
                     .ToList();
            return data;
        }
        public LeaseAgreement GetLeaseById(int id)
        {
            var data = db.LeaseAgreements
                     .Include(l => l.Property)
                     .Include(l => l.Booking)
                     .FirstOrDefault(l => l.LeaseId == id);
            return data;
        }

        public void AcceptLeaseAgreement(int id)
        {
            var lease = db.LeaseAgreements.Find(id);
            if (lease != null)
            {
                lease.LeaseStatus = LeaseStatus.Accepted;
                //lease.AcceptedOn = DateTime.Now;
                db.SaveChanges();
            }
        }

        public void RejectLeaseAgreement(int id)
        {
            var lease = db.LeaseAgreements.Find(id);
            if (lease != null)
            {
                lease.LeaseStatus = LeaseStatus.Terminated;
                //lease.AcceptedOn = DateTime.Now;
                db.SaveChanges();
            }
        }

        public void PaySecurityDeposit(int id)
        {
            var lease = db.LeaseAgreements.Find(id);
            if (lease != null && !lease.IsDepositPaid)
            {
                lease.IsDepositPaid = true;
                db.SaveChanges();

                // Generate initial rent entry for the next month
                db.Rents.Add(new Rent
                {
                    LeaseId = id,
                    DueDate = lease.LeaseStartDate.AddMonths(1),
                    Amount = lease.RentAmount,
                    IsPaid = false
                });
                db.SaveChanges();
            }
        }
        public List<Rent> GetRentByLeaseId(int id)
        {
            var data = db.Rents.Where(r => r.LeaseId == id).OrderBy(r => r.DueDate).ToList();
            return data;
        }
        public Rent GetRentById(int id)
        {
            var data = db.Rents.Include(r => r.LeaseAgreement).FirstOrDefault(r => r.RentId == id);
            return data;
        }
        public void PayRent(int id)
        {
            var rent = db.Rents.Find(id);
            if (rent != null && rent.IsPaid != true)
            {
                rent.IsPaid = true;
                rent.PaidDate = DateTime.Now;
                db.SaveChanges();

                // Schedule next month's rent
                var nextDue = rent.DueDate.AddMonths(1);
                if (!db.Rents.Any(r => r.LeaseId == rent.LeaseId && r.DueDate == nextDue))
                {
                    db.Rents.Add(new Rent
                    {
                        LeaseId = rent.LeaseId,
                        DueDate = nextDue,
                        Amount = rent.Amount,
                        IsPaid = false
                    });
                    db.SaveChanges();
                }
            }
        }
    }
}
