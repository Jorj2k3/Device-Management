using System.ComponentModel.DataAnnotations;


namespace DeviceManagement.Api.Models
{
    /// <summary>
    /// Represents the user class in the Device Management API.
    /// </summary>
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        public ICollection<Device> Devices { get; set; } = new List<Device>();

    }
}
