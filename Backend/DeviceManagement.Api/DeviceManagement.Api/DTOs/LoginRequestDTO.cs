using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object for login requests.
    /// </summary>
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
