using DeviceManagement.Api.Models;

namespace DeviceManagement.Api.DTOs
{
    /// <summary>
    /// Provides extension methods for mapping between Device and DeviceDTO objects.
    /// </summary>
    public static class DeviceMappingExtensions
    {
        public static DeviceDTO ToDTO(this Device device)
        {
            return new DeviceDTO
            {
                Id = device.Id,
                Name = device.Name,
                Manufacturer = device.Manufacturer,
                Type = device.Type,
                OperatingSystem = device.OperatingSystem,
                OsVersion = device.OsVersion,
                Processor = device.Processor,
                RamAmountGb = device.RamAmountGb,
                Description = device.Description,
                AssignedUserID = device.AssignedUserID,
                AssignedUserName = device.AssignedUser?.Name,
                Status = device.Status
            };
        }

        public static Device ToEntity(this DeviceDTO dto)
        {
            return new Device
            {
                Id = dto.Id,
                Name = dto.Name,
                Manufacturer = dto.Manufacturer,
                Type = dto.Type,
                OperatingSystem = dto.OperatingSystem,
                OsVersion = dto.OsVersion,
                Processor = dto.Processor,
                RamAmountGb = dto.RamAmountGb,
                Description = dto.Description,
                AssignedUserID = dto.AssignedUserID,
                Status = dto.Status
            };
        }

        public static IEnumerable<DeviceDTO> ToDTOs(this IEnumerable<Device> devices)
        {
            return devices.Select(d => d.ToDTO());
        }
    }
}
