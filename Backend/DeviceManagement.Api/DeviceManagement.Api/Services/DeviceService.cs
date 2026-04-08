using DeviceManagement.Api.Data;
using DeviceManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Api.Services
{
    /// <summary>
    /// Provides methods for managing device entities, including creation, retrieval, updating, and deletion operations.
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly DeviceDbContext _context;

        public DeviceService(DeviceDbContext context)
        {
            _context = context;
        }

        public async Task<Device> CreateDeviceAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return false;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _context.Devices
                .Include(d => d.AssignedUser)
                .ToListAsync();
        }

        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
            return await _context.Devices
                .Include(d => d.AssignedUser)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Device?> UpdateDeviceAsync(int id, Device device)
        {
            var existingDevice = await _context.Devices.FindAsync(id);
            if (existingDevice == null) return null;

            existingDevice.Name = device.Name;
            existingDevice.Manufacturer = device.Manufacturer;
            existingDevice.Type = device.Type;
            existingDevice.OperatingSystem = device.OperatingSystem;
            existingDevice.OsVersion = device.OsVersion;
            existingDevice.Processor = device.Processor;
            existingDevice.RamAmountGb = device.RamAmountGb;
            existingDevice.Description = device.Description;
            existingDevice.AssignedUserID = device.AssignedUserID;

            await _context.SaveChangesAsync();
            return existingDevice;
        }
    }
}
