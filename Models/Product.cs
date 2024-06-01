using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; } // Binary data of the image

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public IdentityUser User { get; set; } // Navigation property



    }

}
