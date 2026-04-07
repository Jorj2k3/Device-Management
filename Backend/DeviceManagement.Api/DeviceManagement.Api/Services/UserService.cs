using DeviceManagement.Api.Data;
using DeviceManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Api.Services
{
    /// <summary>
    /// Provides methods for managing user entities, including creation, retrieval, updating, and deletion operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly DeviceDbContext _context;

        public UserService(DeviceDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Location = user.Location;

            await _context.SaveChangesAsync();
            return existingUser;
        }
    }
}
