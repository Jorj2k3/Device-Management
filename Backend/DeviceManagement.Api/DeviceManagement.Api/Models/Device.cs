using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DeviceManagement.Api.Models
{
    /// <summary>
    /// Represents the device class in the Device Management API.
    /// </summary>
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string OperatingSystem { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string OsVersion { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Processor { get; set; } = string.Empty;

        [Required]
        public int RamAmountGb { get; set; }

        public string? Description { get; set; }

        public int? AssignedUserID { get; set; }

        [ForeignKey("AssignedUserID")]
        public User? AssignedUser { get; set; }
    }
}
