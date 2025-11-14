using Monitor.Models.Settings;
using Npgsql;

namespace Monitor.Factories
{
    public class PostgresConnection : IPostgresConnection, IDisposable
    {
        private NpgsqlConnection? _connection;
        readonly string _connectionString;
        public PostgresConnection(AppSettings settings)
        {
            _connectionString = settings.Database.ConnectionString ?? 
                throw new ArgumentNullException(nameof(settings.Database.ConnectionString));;
        }

        public async Task<System.Data.Common.DbConnection> GetConnectionAsync()
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(_connectionString);
            }

            if (_connection?.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();    
            }
            
            return _connection;
        }

        public void Dispose()
        {
            if (_connection?.State != System.Data.ConnectionState.Closed)
            {
                _connection?.Close();    
            }

            _connection = null;
        }
    }
}