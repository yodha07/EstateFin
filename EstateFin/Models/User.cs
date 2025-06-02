using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public bool isGoogleUser { get; set; } = false; 
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Role { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        //public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
        //public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        //public virtual ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
        //public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public List<properties>? Properties { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
