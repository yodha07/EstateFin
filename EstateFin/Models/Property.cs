using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateFin.Models
{
    public class Property : IValidatableObject
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price must be zero or greater.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Property type is required.")]
        public string? PropertyType { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Actual asking must be non-negative.")]
        public int OwnerId { get; set; }

        public string? images { get; set; }

        public DateTime? CreatedAt { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public User? User { get; set; }

        public List<Appointment>? Appointments { get; set; }

        // public Booking? Booking { get; set; }

        public List<Review>? Reviews { get; set; } = new List<Review>();

        public LeaseAgreement? lease { get; set; }


        // Custom validation to ensure OwnerId is greater than Price.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check if OwnerId is greater than Price.
            // Note: Compare OwnerId (an int) with Price (a decimal) by converting OwnerId to decimal.
            if (Convert.ToDecimal(OwnerId) <= Price)
            {
                yield return new ValidationResult(
                    "OwnerId must be greater than Price.",
                    new[] { nameof(OwnerId), nameof(Price) }
                );
            }
        }

    }
}