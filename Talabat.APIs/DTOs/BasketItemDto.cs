using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string productName { get; set; }

        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage = "Price must be greater than 0")]
        public decimal price { get; set; }

        [Required]
        public string pictureUrl { get; set; }

        [Required]
        public string category { get; set; }

        [Required]
        public string brand { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "quantity must be greater than 0")]
        public int quantity { get; set; }
    }
}
