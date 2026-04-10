using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;

namespace DeviceManagement.Api.Services
{
    /// <summary>
    /// Defines methods for managing devices: read, create, update, delete.
    /// </summary>
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(int id);
        Task<Device> CreateDeviceAsync(Device device);
        Task<Device?> UpdateDeviceAsync(int id, Device device);
        Task<bool> DeleteDeviceAsync(int id);
        Task<IEnumerable<DeviceDTO>> SearchDevicesAsync(string query);
    }
}
