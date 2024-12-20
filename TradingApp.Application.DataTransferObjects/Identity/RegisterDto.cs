using System.ComponentModel.DataAnnotations;

namespace TradingApp.Application.DataTransferObjects.Identity
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmedPassword { get; set; } = string.Empty;

    }
}
