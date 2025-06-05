using EstateFin.Models.Enum.StatusEnums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class LeaseAgreement
    {
        [Key]
        public int LeaseId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public Property? Property { get; set; } 

        [ForeignKey("Tenant")]
        public int TenantId { get; set; }
        public User? Tenant { get; set; } 

        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking? Booking { get; set; } 

        public DateTime LeaseStartDate { get; set; }
        public DateTime LeaseEndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RentAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SecurityDeposit { get; set; }

        public bool IsDepositPaid { get; set; } = false;

        public LeaseStatus LeaseStatus { get; set; }
    }
}
