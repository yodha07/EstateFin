using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class Review
    {
            [Key]
            public int Id { get; set; }

            public int UserId { get; set; }
            public int PropertyId { get; set; }

            [Required]
            public int Rating { get; set; }
            [Required]
            public string Comment { get; set; }
            public DateTime DatePosted { get; set; }

            // Navigation properties (optional)
            public virtual User User { get; set; }
        //public virtual Property Property { get; set; }

    }
}
