using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class Slot
    {
        [Key] 
        public int Id { get; set; }
        public string Slot_type { get; set; }
        public string Status { get; set; }
    }
}
