using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EstateFin.Models;
using EstateFin.Data;
using EstateFin.ILeaseRepo;


namespace EstateFin.ILeaseRepo
{
    public class LeaseServices : ILeaseRepo
    {
        private readonly ApplicationDbContext context;

        public LeaseServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<LeaseAgreement> GetAll()
        {
            return context.LeaseAgreements
                .Include(x => x.Property)
                .Include(x => x.Tenant)
                .Include(x => x.Booking)
                .ToList();
        }

        public LeaseAgreement GetById(int id)
        {
            return context.LeaseAgreements
                .Include(x => x.Property)
                .Include(x => x.Tenant)
                .Include(x => x.Booking)
                .FirstOrDefault(x => x.LeaseId == id);
        }

        public void Add(LeaseAgreement lease)
        {
            context.LeaseAgreements.Add(lease);
        }

        public void Update(LeaseAgreement lease)
        {
            context.LeaseAgreements.Update(lease);
        }

        public void Delete(int id)
        {
            var lease = GetById(id);
            if (lease != null)
            {
                context.LeaseAgreements.Remove(lease);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public int getId(int id)
        {
            var lease = context.LeaseAgreements.Find(id);
            return lease?.LeaseId ?? 0;
        }

        public void Del(int id)
        {
            var item = context.LeaseAgreements.Find(id);
            if (item != null)
            {
                context.LeaseAgreements.Remove(item); // use the entity set, not just context.Remove
                context.SaveChanges(); // ✅ Save changes to the DB
            }
        }
    }
}
