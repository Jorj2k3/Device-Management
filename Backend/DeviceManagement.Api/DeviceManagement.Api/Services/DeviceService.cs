using DeviceManagement.Api.Data;
using DeviceManagement.Api.DTOs;
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
            existingDevice.Status = device.Status;

            await _context.SaveChangesAsync();
            return existingDevice;
        }

        public async Task<IEnumerable<DeviceDTO>> SearchDevicesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                var all = await GetAllDevicesAsync();
                return (IEnumerable<DeviceDTO>)await GetAllDevicesAsync();
            }

            var delimiters = new char[] { ' ', ',', '.', ';', ':', '-', '_' };
            var tokens = query.ToLowerInvariant()
                              .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                              .Distinct()
                              .ToList();

            var allDevices = await _context.Devices.Include(d => d.AssignedUser).ToListAsync();

            var matchingDevices = allDevices.Select(device =>
            {
                int score = 0;
                string name = device.Name?.ToLowerInvariant() ?? "";
                string manufacturer = device.Manufacturer?.ToLowerInvariant() ?? "";
                string type = device.Type?.ToLowerInvariant() ?? "";
                string operatingSystem = device.OperatingSystem?.ToLowerInvariant() ?? "";
                string processor = device.Processor?.ToLowerInvariant() ?? "";
                string ram = device.RamAmountGb.ToString();

                foreach (var token in tokens)
                {
                    if (name.Contains(token)) score += 10;
                    if (manufacturer.Contains(token)) score += 5;
                    if (type.Contains(token)) score += 3;
                    if (operatingSystem.Contains(token)) score += 2;
                    if (processor.Contains(token)) score += 1;
                    if (ram.Contains(token)) score += 1;
                }

                return new { Device = device, Score = score };
            })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Device.Name ?? "")
                .Select(x => x.Device.ToDTO())
                .ToList();

            return matchingDevices;
        }
    }
}
