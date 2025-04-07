using Npgsql;
using Microsoft.Extensions.Configuration;

namespace RegistrationAPI.DataAccess
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
} 