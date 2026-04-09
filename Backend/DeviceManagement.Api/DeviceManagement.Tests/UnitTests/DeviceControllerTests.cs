using System;
using System.Collections.Generic;
using System.Text;
using DeviceManagement.Api.Controllers;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DeviceManagement.Tests.UnitTests
{
    /// <summary>
    /// Contains unit tests for the DeviceController, verifying its behavior for device retrieval and creation scenarios.
    /// </summary>
    public class DeviceControllerTests
    {
        [Fact]
        public async Task GetAllDevices_ReturnOkResult_WithListOfDevices()
        {
            var mockService = new Mock<IDeviceService>();

            var fakeDevices = new List<Device>
            {
                new Device { Id = 1, Name = "Laptop A", Manufacturer = "BrandX", Type = "Laptop", OperatingSystem = "Windows", OsVersion = "10", Processor = "Intel i5", RamAmountGb = 8, Description = "Office laptop", AssignedUserID = 1 },
                new Device { Id = 2, Name = "Phone B", Manufacturer = "BrandY", Type = "Smartphone", OperatingSystem = "Android", OsVersion = "11", Processor = "Snapdragon 888", RamAmountGb = 6, Description = "Employee phone", AssignedUserID = 2 }
            };

            mockService.Setup(service => service.GetAllDevicesAsync())
                       .ReturnsAsync(fakeDevices);

            var mockAiService = new Mock<IAiService>();

            var controller = new DeviceController(mockService.Object, mockAiService.Object);

            var result = await controller.GetDevices();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<DeviceDTO>>(okResult.Value);
            Assert.Equal(2, returnedDtos.Count());
        }

        [Fact]
        public async Task GetDevice_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var mockService = new Mock<IDeviceService>();
            int fakeId = 999;

            mockService.Setup(service => service.GetDeviceByIdAsync(fakeId))
                       .ReturnsAsync((Device?)null);
            var mockAiService = new Mock<IAiService>();
            var controller = new DeviceController(mockService.Object, mockAiService.Object);

            var result = await controller.GetDevice(fakeId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateDevice_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var mockService = new Mock<IDeviceService>();
            var mockAiService = new Mock<IAiService>();
            var controller = new DeviceController(mockService.Object, mockAiService.Object);
            controller.ModelState.AddModelError("Name", "The Name field is required.");
            var newDeviceDto = new DeviceDTO
            {
                // Name is missing to trigger model validation error
                Manufacturer = "BrandZ",
                Type = "Tablet",
                OperatingSystem = "iOS",
                OsVersion = "14",
                Processor = "Apple A14",
                RamAmountGb = 4,
                Description = "Test tablet",
                AssignedUserID = 3
            };
            var result = await controller.CreateDevice(newDeviceDto);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateDevice_ReturnsCreatedResult_WithCorrectType()
        {
            var mockService = new Mock<IDeviceService>();

            var newDeviceDto = new DeviceDTO
            {
                Name = "New Laptop",
                Manufacturer = "BrandX",
                Type = "Laptop",
                OperatingSystem = "Windows",
                OsVersion = "10",
                Processor = "Intel i7",
                RamAmountGb = 16,
                Description = "High-end laptop for development",
                AssignedUserID = 4
            };

            mockService.Setup(service => service.CreateDeviceAsync(It.IsAny<Device>()))
                       .ReturnsAsync((Device device) => device);
            var mockAiService = new Mock<IAiService>();

            var controller = new DeviceController(mockService.Object, mockAiService.Object);

            var result = await controller.CreateDevice(newDeviceDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDto = Assert.IsType<DeviceDTO>(createdAtActionResult.Value);
            Assert.Equal("Laptop", returnedDto.Type);
        }

    }
}
