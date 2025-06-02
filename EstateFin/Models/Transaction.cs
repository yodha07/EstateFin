using EstateFin.Models.Enum.StatusEnums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        public string PaymentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // Credit Card, UPI, Bank Transfer, Cash

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public Booking Booking { get; set; } // Navigation property
    }
}
