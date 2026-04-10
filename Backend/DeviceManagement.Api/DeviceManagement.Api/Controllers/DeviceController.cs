using Asp.Versioning;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Api.Controllers
{
    /// <summary>
    /// DeviceController is responsible for handling HTTP requests related to device management,
    /// including creating, retrieving, updating, and deleting device records. 
    /// It interacts with the IDeviceService to perform these operations and returns appropriate HTTP responses based on the outcome of each request.
    /// </summary>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]s")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IAiService _aiService;

        public DeviceController(IDeviceService deviceService, IAiService aiService)
        {
            _deviceService = deviceService;
            _aiService = aiService;
        }

        // GET: api/Devices
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevices()
        {
            try
            {
                var devices = await _deviceService.GetAllDevicesAsync();

                return Ok(devices.ToDTOs());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred processing your request.");
            }
        }

        // GET: api/Devices/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceDTO>> GetDevice(int id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);

                if (device == null)
                {
                    return NotFound();
                }

                return Ok(device.ToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred processing your request.");
            }
        }

        // POST: api/Devices
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceDTO>> CreateDevice(DeviceDTO deviceDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newDevice = deviceDTO.ToEntity();

                var createdDevice = await _deviceService.CreateDeviceAsync(newDevice);
                deviceDTO.Id = createdDevice.Id;

                return CreatedAtAction(nameof(GetDevice), new { id = createdDevice.Id }, deviceDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred processing your request.");
            }
        }

        // PUT: api/Devices/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDevice(int id, DeviceDTO deviceDTO)
        {
            if (id != deviceDTO.Id)
            {
                return BadRequest("Device ID mismatch.");
            }

            try
            {
                var deviceToUpdate = deviceDTO.ToEntity();

                var updatedDevice = await _deviceService.UpdateDeviceAsync(id, deviceToUpdate);

                if (updatedDevice == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred processing your request.");
            }
        }

        // DELETE: api/devices/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                var success = await _deviceService.DeleteDeviceAsync(id);
                if (!success) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred processing your request.");
            }
        }

        [HttpPost("GenerateDescription")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GenerateDescription([FromBody] DeviceDTO specs)
        {
            var description = await _aiService.GenerateDeviceDescriptionAsync(
                specs.Name, specs.Manufacturer, specs.OperatingSystem,
                specs.Type, specs.RamAmountGb, specs.Processor);

            return Ok(new { description = description });
        }

        [HttpGet("Search")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> SearchDevices([FromQuery] string? q)
        {
            var devices = await _deviceService.SearchDevicesAsync(q);
            return Ok(devices);
        }
    }
}
