using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EstateFin.Models.Enum.StatusEnums;

namespace EstateFin.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public decimal Amount { get; set; }


        
        public User User { get; set; }           // Navigation property

        public Property Property { get; set; } 

             
    }
}
