using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFin.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string? PropertyType { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public string? images { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("User")]
        public int UserID {  get; set; }

        public User? User { get; set; }

        public List<Appointment>? Appointments { get; set; }
        //public Booking? Booking { get; set; }

        public List<Review>? Reviews { get; set; } = new List<Review>();

        public LeaseAgreement? lease { get; set; }


//:EstateFin/Models/Property.cs

    }
}
