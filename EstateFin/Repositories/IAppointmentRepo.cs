using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface IAppointmentRepo
    {

        //List<Appointment> appointments(int user);
        List<int> PropertyIdNull(int user);
        //List<Appointment> appointments();
        Appointment GetAppointment(int id);
        void SaveChanges();
        void DeleteChanges(Appointment user);
        List<Slot> GetSlots();
        List<User> GetUser(int id);
    }
}
