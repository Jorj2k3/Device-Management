using DeviceManagement.Api.Controllers;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace DeviceManagement.Tests.IntegrationTests
{
    public class DevicesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public DevicesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
            db.Devices.RemoveRange(db.Devices);
            await db.SaveChangesAsync();
        }
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetAllDevices_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var response = await _client.GetAsync("/api/Devices");

            response.EnsureSuccessStatusCode();

            var devices = await response.Content.ReadFromJsonAsync<List<DeviceDTO>>();
            Assert.NotNull(devices);
            Assert.Empty(devices);
        }

        [Fact]
        public async Task PostDevice_AndThenGetDevice_ReturnSavedDevice()
        {
            var newDevice = new DeviceDTO
            {
                Name = "Integration Test Device",
                Manufacturer = "TestBrand",
                Type = "Laptop",
                OperatingSystem = "TestOS",
                OsVersion = "1.0",
                Processor = "TestProcessor",
                RamAmountGb = 16,
                Description = "Device created during integration testing",
                AssignedUserID = null
            };
            var postResponse = await _client.PostAsJsonAsync("/api/Devices", newDevice);
            postResponse.EnsureSuccessStatusCode();

            var createdDevice = await postResponse.Content.ReadFromJsonAsync<DeviceDTO>();
            Assert.NotNull(createdDevice);
            int newDeviceId = createdDevice.Id;

            var getResponse = await _client.GetAsync($"/api/Devices/{newDeviceId}");
            getResponse.EnsureSuccessStatusCode();

            var retrievedDevice = await getResponse.Content.ReadFromJsonAsync<DeviceDTO>();

            Assert.NotNull(retrievedDevice);
            Assert.Equal(newDevice.Name, retrievedDevice.Name);
            Assert.Equal(newDevice.Manufacturer, retrievedDevice.Manufacturer);
            Assert.Equal(newDevice.Type, retrievedDevice.Type);
            Assert.Equal(newDevice.OperatingSystem, retrievedDevice.OperatingSystem);
            Assert.Equal(newDevice.OsVersion, retrievedDevice.OsVersion);
            Assert.Equal(newDevice.Processor, retrievedDevice.Processor);
            Assert.Equal(newDevice.RamAmountGb, retrievedDevice.RamAmountGb);
            Assert.Equal(newDevice.Description, retrievedDevice.Description);
            Assert.Equal(newDevice.AssignedUserID, retrievedDevice.AssignedUserID);
        }

        [Fact]
        public async Task GetDevice_ReturnsNotFound_WhenIdDoesNotExist()
        {
            int fakeId = 999;
            var response = await _client.GetAsync($"/api/Devices/{fakeId}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task CreateDevice_AndThenUpdateDevice_ReturnsDevice()
        {
            var newDevice = new DeviceDTO
            {
                Name = "Integration Test Device",
                Manufacturer = "TestBrand",
                Type = "Laptop",
                OperatingSystem = "TestOS",
                OsVersion = "1.0",
                Processor = "TestProcessor",
                RamAmountGb = 16,
                Description = "Device created during integration testing",
                AssignedUserID = null
            };
            var postResponse = await _client.PostAsJsonAsync("/api/Devices", newDevice);
            postResponse.EnsureSuccessStatusCode();

            var createdDevice = await postResponse.Content.ReadFromJsonAsync<DeviceDTO>();
            Assert.NotNull(createdDevice);
            int updateDeviceId = createdDevice.Id;

            var updatedDevice = new DeviceDTO
            {
                Id = updateDeviceId,
                Name = "Updated Test Device",
                Manufacturer = "UpdatedTestBrand",
                Type = "Desktop",
                OperatingSystem = "UpdatedTestOS",
                OsVersion = "2.0",
                Processor = "UpdatedTestProcessor",
                RamAmountGb = 32,
                Description = "Device updated during integration testing",
                AssignedUserID = null
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/Devices/{updateDeviceId}", updatedDevice);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/Devices/{updateDeviceId}");
            getResponse.EnsureSuccessStatusCode();

            var retrievedDevice = await getResponse.Content.ReadFromJsonAsync<DeviceDTO>();
            
            Assert.NotNull(retrievedDevice);
            Assert.Equal(updatedDevice.Name, retrievedDevice.Name);
            Assert.Equal(updatedDevice.Manufacturer, retrievedDevice.Manufacturer);
            Assert.Equal(updatedDevice.Type, retrievedDevice.Type);
            Assert.Equal(updatedDevice.OperatingSystem, retrievedDevice.OperatingSystem);
            Assert.Equal(updatedDevice.OsVersion, retrievedDevice.OsVersion);
            Assert.Equal(updatedDevice.Processor, retrievedDevice.Processor);
            Assert.Equal(updatedDevice.RamAmountGb, retrievedDevice.RamAmountGb);
            Assert.Equal(updatedDevice.Description, retrievedDevice.Description);
            Assert.Equal(updatedDevice.AssignedUserID, retrievedDevice.AssignedUserID);
        }

        [Fact]
        public async Task UpdateDevice_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var mockService = new Mock<IDeviceService>();
            var controller = new DeviceController(mockService.Object);

            int urlId = 5;
            var bodyDto = new DeviceDTO { Id = 99, Name = "Mismatched Device" };

            var result = await controller.UpdateDevice(urlId, bodyDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Device ID mismatch.", badRequestResult.Value);

            mockService.Verify(service => service.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Never);
        }

        [Fact]
        public async Task CreateDevice_AndThenDelete_ReturnsEmptyList()
        {
            var newDevice = new DeviceDTO
            {
                Name = "Integration Test Device",
                Manufacturer = "TestBrand",
                Type = "Laptop",
                OperatingSystem = "TestOS",
                OsVersion = "1.0",
                Processor = "TestProcessor",
                RamAmountGb = 16,
                Description = "Device created during integration testing",
                AssignedUserID = null
            };

            var postResponse = await _client.PostAsJsonAsync("/api/Devices", newDevice);
            postResponse.EnsureSuccessStatusCode();

            var createdDevice = await postResponse.Content.ReadFromJsonAsync<DeviceDTO>();
            Assert.NotNull(createdDevice);
            int deleteDeviceId = createdDevice.Id;

            var deleteResponse = await _client.DeleteAsync($"/api/Devices/{deleteDeviceId}");
            deleteResponse.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/Devices");
            response.EnsureSuccessStatusCode();

            var devices = await response.Content.ReadFromJsonAsync<List<DeviceDTO>>();

            Assert.NotNull(devices);
            Assert.Empty(devices);
        }
    }
}
