using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Api.Controllers
{
    /// <summary>
    /// DeviceController is responsible for handling HTTP requests related to device management,
    /// including creating, retrieving, updating, and deleting device records. 
    /// It interacts with the IDeviceService to perform these operations and returns appropriate HTTP responses based on the outcome of each request.
    /// </summary>

    [Route("api/[controller]s")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();

            var devicesDTO = devices.Select(d => new DeviceDTO
            {
                Id = d.Id,
                Name = d.Name,
                Manufacturer = d.Manufacturer,
                Type = d.Type,
                OperatingSystem = d.OperatingSystem,
                OsVersion = d.OsVersion,
                Processor = d.Processor,
                RamAmountGb = d.RamAmountGb,
                Description = d.Description,
                AssignedUserID = d.AssignedUserID
            });

            return Ok(devicesDTO);
        }

        // GET: api/Devices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDTO>> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            var deviceDTO = new DeviceDTO
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
                AssignedUserID = device.AssignedUserID
            };

            return Ok(deviceDTO);
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<ActionResult<DeviceDTO>> CreateDevice(DeviceDTO deviceDTO)
        {
            var newDevice = new Device
            {
                Name = deviceDTO.Name,
                Manufacturer = deviceDTO.Manufacturer,
                Type = deviceDTO.Type,
                OperatingSystem = deviceDTO.OperatingSystem,
                OsVersion = deviceDTO.OsVersion,
                Processor = deviceDTO.Processor,
                RamAmountGb = deviceDTO.RamAmountGb,
                Description = deviceDTO.Description,
                AssignedUserID = deviceDTO.AssignedUserID
            };

            var createdDevice = await _deviceService.CreateDeviceAsync(newDevice);
            deviceDTO.Id = createdDevice.Id;

            return CreatedAtAction(nameof(GetDevice), new { id = createdDevice.Id }, deviceDTO);
        }

        // PUT: api/Devices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, DeviceDTO deviceDTO)
        {
            if (id != deviceDTO.Id)
            {
                return BadRequest("Device ID mismatch.");
            }

            var deviceToUpdate = new Device
            {
                Id = deviceDTO.Id,
                Name = deviceDTO.Name,
                Manufacturer = deviceDTO.Manufacturer,
                Type = deviceDTO.Type,
                OperatingSystem = deviceDTO.OperatingSystem,
                OsVersion = deviceDTO.OsVersion,
                Processor = deviceDTO.Processor,
                RamAmountGb = deviceDTO.RamAmountGb,
                Description = deviceDTO.Description,
                AssignedUserID = deviceDTO.AssignedUserID
            };

            var updatedDevice = await _deviceService.UpdateDeviceAsync(id, deviceToUpdate);

            if (updatedDevice == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/devices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            await _deviceService.DeleteDeviceAsync(id);

            return NoContent();
        }
    }
}
