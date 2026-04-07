using DeviceManagement.Api.Controllers;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DeviceManagement.Tests.UnitTests
{
    /// <summary>
    /// Contains unit tests for the UserController, verifying its behavior for user retrieval and creation scenarios.
    /// </summary>
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsers_ReturnOkResult_WithListOfUsers()
        {
            var mockService = new Mock<IUserService>();

            var fakeUsers = new List<User>
            {
                new User { Id = 1, Name = "Alice", Email = "alice@test.com", Role = "Admin", Location = "HQ" },
                new User { Id = 2, Name = "Bob", Email = "bob@test.com", Role = "Employee", Location = "Remote" }
            };

            mockService.Setup(service => service.GetAllUsersAsync())
                       .ReturnsAsync(fakeUsers);

            var controller = new UserController(mockService.Object);

            var result = await controller.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);

            Assert.Equal(2, returnedDtos.Count());
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var mockService = new Mock<IUserService>();
            int fakeId = 999;

            mockService.Setup(service => service.GetUserByIdAsync(fakeId))
                       .ReturnsAsync((User?)null);

            var controller = new UserController(mockService.Object);

            var result = await controller.GetUser(fakeId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedResult_AndAssignsEmployeeRole()
        {
            var mockService = new Mock<IUserService>();

            var incomingDto = new UserDTO
            {
                Name = "Sneaky Hacker",
                Email = "hacker@test.com",
                Role = "Admin",
                Location = "Basement"
            };

            var savedUser = new User
            {
                Id = 5,
                Name = "Sneaky Hacker",
                Email = "hacker@test.com",
                Role = "Employee",
                Location = "Basement"
            };

            mockService.Setup(service => service.CreateUserAsync(It.IsAny<User>()))
                       .ReturnsAsync(savedUser);

            var controller = new UserController(mockService.Object);

            var result = await controller.CreateUser(incomingDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            var returnedDto = Assert.IsType<UserDTO>(createdResult.Value);

            Assert.Equal(5, returnedDto.Id);
            Assert.Equal("Employee", returnedDto.Role);
        }
    }
}
