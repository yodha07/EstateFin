using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;

namespace EstateFin.Services
{
    public class AppointmentService: IAppointmentRepo
    {
        private readonly ApplicationDbContext db;
        public AppointmentService(ApplicationDbContext db)
        {
            this.db = db;
        }
        //public List<Appointment> appointments(int user)
        //{
        //    return List<Appointment>;
        //}

        public List<int> PropertyIdNull(int user)
        {
            return db.Properties
                     .Where(p => p.UserID == user)
                     .Select(p => p.PropertyId)
                     .ToList();
        }

        //public List<Appointment> appointments()
        //{
        //    db.appointment
        //       .Where(x => propertyIds.Contains(x.PropertyId) && x.Status.Equals("pending"))
        //       .Include(x => x.Property)
        //       .Include(x => x.User)
        //       .ToList();
        //}
        public Appointment GetAppointment(int id)
        {
            return db.appointment.Find(id);
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
        public void DeleteChanges(Appointment user)
        {
            db.Remove(user);
        }
       public List<Slot> GetSlots()
        {
            return db.slot.Where(x => x.Status.Equals("Active")).ToList();
        }

        public List<User> GetUser(int id)
        {
             

            return db.Users.Where(x => x.UserID.Equals(id)).ToList();
        }
    }
}
