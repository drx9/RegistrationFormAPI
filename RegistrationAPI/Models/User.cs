using System;

namespace RegistrationAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? Data { get; set; }
    }

    public class UsersResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<User> Data { get; set; } = new();
    }
} 