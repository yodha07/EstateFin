using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFin.Models
{
    public class Review
    {
            [Key]
            public int Id { get; set; }

            [ForeignKey("User")]
            public int UserId { get; set; }

            [ForeignKey("properties")]

            public int PropertyId { get; set; }

            [Required]
            public int Rating { get; set; }
            [Required]
            public string Comment { get; set; }
            public DateTime DatePosted { get; set; }

            public User User { get; set; }
            public properties properties { get; set; }

    }
}
