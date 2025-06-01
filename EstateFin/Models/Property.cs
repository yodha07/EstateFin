using System.ComponentModel.DataAnnotations;

namespace EstateFin.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        //public virtual ICollection<Review> Reviews { get; } = new List<Review>();
        public List<Review> Reviews { get; set; }
    }
}
