using DeviceManagement.Api.Models;

namespace DeviceManagement.Api.Utilities
{
    /// <summary>
    /// Interface for generating authentication tokens for users. 
    /// Implementations of this interface will create JWT tokens.
    /// </summary>
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
