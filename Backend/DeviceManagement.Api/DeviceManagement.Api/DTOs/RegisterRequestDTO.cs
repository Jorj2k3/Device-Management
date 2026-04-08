using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object for registration requests.
    /// </summary>
    public class RegisterRequestDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}
