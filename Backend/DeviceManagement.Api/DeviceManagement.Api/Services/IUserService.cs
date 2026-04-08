using DeviceManagement.Api.Models;

namespace DeviceManagement.Api.Services
{
    /// <summary>
    /// Defines methods for managing user accounts, including retrieval, creation, update, and deletion operations.
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
