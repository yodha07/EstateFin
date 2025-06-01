using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class Property_Type
    {

        [Key]
        public int MyPropertyId { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string? PropertyType { get; set; }

        [Required(ErrorMessage = "Required Field")]

        public string? status { get; set; }

        public string? createdby { get; set; }
        public DateTime? createdat { get; set; }
    }
}
