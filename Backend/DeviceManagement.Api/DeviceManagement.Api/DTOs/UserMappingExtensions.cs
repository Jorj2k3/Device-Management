using DeviceManagement.Api.Models;

namespace DeviceManagement.Api.DTOs
{
    public static class UserMappingExtensions
    {
        public static UserDTO ToDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Location = user.Location
            };
        }

        public static User ToEntity(this UserDTO dto)
        {
            return new User
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                Location = dto.Location
            };
        }

        public static IEnumerable<UserDTO> ToDTOs(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDTO());
        }
    }
}
