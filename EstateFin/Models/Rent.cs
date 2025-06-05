using EstateFin.Models.Enum.StatusEnums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFin.Models
{

    public class Rent
    {
        public int RentId { get; set; }

    [ForeignKey("LeaseAgreement")]
    public int LeaseId { get; set; }
    public LeaseAgreement LeaseAgreement { get; set; }

    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal? Amount { get; set; }
    public bool IsPaid { get; set; }
    public RentStatus? RentStatus { get; set; }
}
}

