using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string PassWord { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
