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
    /// <summary>
    /// Provides integration tests for user-related API endpoints using a test web application factory.
    /// </summary>
    public class UsersIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public UsersIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
            db.Users.RemoveRange(db.Users);
            await db.SaveChangesAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetAllUsers_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var response = await _client.GetAsync("/api/v1/Users");

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            Assert.NotNull(users);
            Assert.Empty(users);
        }

        [Fact]
        public async Task PostUser_AndThenGetUser_ReturnsSavedUser()
        {
            var newUser = new UserDTO
            {
                Name = "Integration Tester",
                Email = "test@system.com",
                Role = "Hacker",
                Location = "Server Room"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Users", newUser);
            postResponse.EnsureSuccessStatusCode();

            var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(createdUser);
            int newUserId = createdUser.Id;

            var getResponse = await _client.GetAsync($"/api/v1/Users/{newUserId}");
            getResponse.EnsureSuccessStatusCode();

            var retrievedUser = await getResponse.Content.ReadFromJsonAsync<UserDTO>();

            Assert.NotNull(retrievedUser);
            Assert.Equal("Integration Tester", retrievedUser.Name);
            Assert.Equal("Employee", retrievedUser.Role);
        }

        [Fact]
        public async Task CreateUser_AndThenUpdateUser_ReturnsSavedUser()
        {
            var newUser = new UserDTO
            {
                Name = "Integration Tester",
                Email = "test@system.com",
                Role = "Employee",
                Location = "Server Room"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Users", newUser);
            postResponse.EnsureSuccessStatusCode();

            var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(createdUser);
            int updateUserId = createdUser.Id;

            var updatedUser = new UserDTO
            {
                Id = updateUserId,
                Name = "Updated Tester",
                Email = "test@system.com",
                Role = "Employee",
                Location = "Server Room"
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/Users/{updateUserId}", updatedUser);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/v1/Users/{updateUserId}");
            getResponse.EnsureSuccessStatusCode();

            var retrievedUser = await getResponse.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(retrievedUser);
            Assert.Equal("Updated Tester", retrievedUser.Name);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var mockService = new Mock<IUserService>();
            var controller = new UserController(mockService.Object);

            int urlId = 5;
            var bodyDto = new UserDTO { Id = 99, Name = "Mismatched User" };

            var result = await controller.UpdateUser(urlId, bodyDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The ID does not match the request body.", badRequestResult.Value);

            mockService.Verify(service => service.UpdateUserAsync(It.IsAny<int>(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateUser_AndThenDelete_ReturnsEmptyList()
        {
            var newUser = new UserDTO
            {
                Name = "Integration Tester",
                Email = "test@system.com",
                Role = "Employee",
                Location = "Server Room"
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/Users", newUser);
            postResponse.EnsureSuccessStatusCode();

            var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(createdUser);
            int deleteUserId = createdUser.Id;

            var deleteResponse = await _client.DeleteAsync($"/api/v1/Users/{deleteUserId}");
            deleteResponse.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/v1/Users");

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            Assert.NotNull(users);
            Assert.Empty(users);
        }
    }
}
