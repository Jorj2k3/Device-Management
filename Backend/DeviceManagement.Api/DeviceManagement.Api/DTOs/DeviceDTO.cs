namespace DeviceManagement.Api.DTOs
{
    /// <summary>
    /// Data Transfer Object for Device entity.
    /// </summary>
    public class DeviceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string OsVersion { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public int RamAmountGb { get; set; }
        public string? Description { get; set; }
        public int? AssignedUserID { get; set; }
        public string? AssignedUserName { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}
