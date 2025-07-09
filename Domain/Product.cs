using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name can't exceed 100 characters.")]
        public string Name { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Price must be at least 0.01.")]
        public float Price { get; set; }
    }
}
