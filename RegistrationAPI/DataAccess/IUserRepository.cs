using RegistrationAPI.Models;

namespace RegistrationAPI.DataAccess
{
    public interface IUserRepository
    {
        Task<UserResponse> CreateUserAsync(User user);
        Task<UsersResponse> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> UpdateUserAsync(int id, User user);
        Task<UserResponse> DeleteUserAsync(int id);
    }
} 