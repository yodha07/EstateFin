using EstateFin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EstateFin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<properties> Properties { get; set; }
        public DbSet<Property_Type> Property_Types { get; set; }

        //public DbSet<Booking> Bookings { get; set; }
        //public DbSet<Property> Properties { get; set; }
        //public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<LeaseAgreement> LeaseAgreements { get; set; }
        //public DbSet<Review> Reviews { get; set; }
    }
}
