using Npgsql;

namespace RegistrationAPI.DataAccess
{
    public interface IDatabaseConnection
    {
        NpgsqlConnection GetConnection();
    }
} 