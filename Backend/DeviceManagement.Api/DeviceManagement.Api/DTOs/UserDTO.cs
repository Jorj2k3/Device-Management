namespace DeviceManagement.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object for User entity.
    /// </summary>
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public ICollection<DeviceDTO> Devices { get; set; } = new List<DeviceDTO>();
    }
}
    