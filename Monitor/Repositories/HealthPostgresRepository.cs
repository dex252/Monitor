using Dapper;
using Monitor.Factories;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Repositories
{
    public class HealthPostgresRepository : BaseRepository, IHealthDbRepository
    {
        public HealthPostgresRepository(IPostgresConnection dbConnector) : base(dbConnector)
        {
        }

        public async Task<bool> CheckConnection()
        {
            var sql = "SELECT 1";

            try
            {
                return await ExecuteAsync(async connection =>
            {
                var result = await connection.ExecuteScalarAsync<int>(sql);
                return result == 1;
            });
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}