using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public bool isGoogleUser { get; set; } = false;
        
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be more than 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be more than 100 characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? PhoneNumber { get; set; }

        [Required (ErrorMessage = "Role is required.")]
        public string? Role { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
       
        public List<Booking>? Booking { get; set; }

        public List<Transaction>? Transactions { get; set; } = new List<Transaction>();
        public List<Property>? Properties { get; set; }

        public List<Review>? Reviews { get; set; } = new List<Review>();

        public Appointment? Appointment { get; set; }
    }
}


//public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
//public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
//public virtual ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
//public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();



//public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
//public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
//public virtual ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
//public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
