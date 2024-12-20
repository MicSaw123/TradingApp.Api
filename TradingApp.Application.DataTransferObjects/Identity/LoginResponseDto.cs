using System.ComponentModel.DataAnnotations;

namespace TradingApp.Application.DataTransferObjects.Identity
{
    public class LoginResponseDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
