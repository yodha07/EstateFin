using EstateFin.Models.Enum.StatusEnums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFin.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public string? Status { get; set; }

        public string slot { get; set; }

    }
}
