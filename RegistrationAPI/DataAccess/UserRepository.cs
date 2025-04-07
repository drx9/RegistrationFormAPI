using Npgsql;
using RegistrationAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace RegistrationAPI.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public UserRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<UserResponse> CreateUserAsync(User user)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT * FROM sp_create_user($1::varchar, $2::varchar, $3::varchar, $4::varchar)", conn))
                {
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, user.FullName);
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, user.Email);
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, user.Phone);
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, HashPassword(user.Password));

                    try
                    {
                        using var reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            return new UserResponse
                            {
                                Success = true,
                                Message = "User created successfully",
                                Data = new User
                                {
                                    Id = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.GetString(3),
                                    CreatedAt = reader.GetDateTime(4)
                                }
                            };
                        }
                    }
                    catch (PostgresException ex)
                    {
                        return new UserResponse
                        {
                            Success = false,
                            Message = ex.Message
                        };
                    }
                }
            }
            return new UserResponse { Success = false, Message = "Failed to create user" };
        }

        public async Task<UsersResponse> GetAllUsersAsync()
        {
            var response = new UsersResponse { Data = new List<User>() };
            using (var conn = _dbConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT * FROM public.sp_get_all_users()", conn))
                {
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        response.Data.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            CreatedAt = reader.GetDateTime(4)
                        });
                    }
                }
            }
            response.Success = true;
            response.Message = "Users retrieved successfully";
            return response;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT * FROM public.sp_get_user_by_id($1)", conn))
                {
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, id);
                    var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return new UserResponse
                        {
                            Success = true,
                            Message = "User retrieved successfully",
                            Data = new User
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Email = reader.GetString(2),
                                Phone = reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4)
                            }
                        };
                    }
                }
            }
            return new UserResponse { Success = false, Message = "User not found" };
        }

        public async Task<UserResponse> UpdateUserAsync(int id, User user)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT * FROM sp_update_user($1::integer, $2::varchar, $3::varchar, $4::varchar, $5::varchar)", conn))
                {
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, id);
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, user.FullName);
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, user.Email);
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, user.Phone);
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, HashPassword(user.Password));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, DBNull.Value);
                    }

                    try
                    {
                        var reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            return new UserResponse
                            {
                                Success = true,
                                Message = "User updated successfully",
                                Data = new User
                                {
                                    Id = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.GetString(3),
                                    CreatedAt = reader.GetDateTime(4)
                                }
                            };
                        }
                    }
                    catch (PostgresException ex)
                    {
                        return new UserResponse
                        {
                            Success = false,
                            Message = ex.Message
                        };
                    }
                }
            }
            return new UserResponse { Success = false, Message = "Failed to update user" };
        }

        public async Task<UserResponse> DeleteUserAsync(int id)
        {
            using (var conn = _dbConnection.GetConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT public.sp_delete_user($1)", conn))
                {
                    cmd.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, id);
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null && Convert.ToInt32(result) > 0)
                    {
                        return new UserResponse
                        {
                            Success = true,
                            Message = "User deleted successfully"
                        };
                    }
                }
            }
            return new UserResponse { Success = false, Message = "Failed to delete user" };
        }
    }
} 