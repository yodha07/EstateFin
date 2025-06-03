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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany()
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            //    modelBuilder.Entity<Appointment>()
            //.HasOne(a => a.User)
            //.WithMany()
            //.HasForeignKey(a => a.UserId)
            //.OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Appointment>()
            //    .HasOne(a => a.Property)
            //    .WithMany()
            //    .HasForeignKey(a => a.PropertyId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Transaction: Booking ↔ Transactions
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Booking)
                .WithMany()
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Lease: Property ↔ Leases
            //modelBuilder.Entity<Lease>()
            //    .HasOne(l => l.Property)
            //    .WithMany()
            //    .HasForeignKey(l => l.PropertyId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Lease>()
            //    .HasOne(l => l.Tenant)
            //    .WithMany()
            //    .HasForeignKey(l => l.TenantId)
            //    .OnDelete(DeleteBehavior.Cascade);


            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r.PropertyId)
            //    .WithMany()
            //    .HasForeignKey(r => r.PropertyId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Review>()
                .HasOne(r => r.Property)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascading delete

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany()
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascading delete

        }


        public DbSet<User> Users { get; set; }


        public DbSet<Review> Reviews { get; set; }

        public DbSet<Models.Property> Properties { get; set; }
        public DbSet<Property_Type> Property_Types { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

            
        //}



        //public DbSet<Booking> Bookings { get; set; }

        //public DbSet<Transaction> Transactions { get; set; }

        //public DbSet<Property> Properties { get; set; }
        //public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<LeaseAgreement> LeaseAgreements { get; set; }
        //public DbSet<Review> Reviews { get; set; }
    }
}
