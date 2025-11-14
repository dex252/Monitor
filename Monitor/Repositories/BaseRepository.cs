namespace Monitor.Repositories
{
    public abstract class BaseRepository
    {
        readonly Factories.IDbConnection _dbConnector;
        public BaseRepository(Factories.IDbConnection dbConnector)
        {
            _dbConnector = dbConnector;
        }

        async Task<System.Data.IDbConnection> GetConnectionAsync()
        {
            return await _dbConnector.GetConnectionAsync();
        }

        protected async Task<T> ExecuteAsync<T>(Func<System.Data.IDbConnection, Task<T>> operation)
        {
            var connection = await GetConnectionAsync();
            return await operation(connection);
        }

        protected async Task ExecuteAsync(Func<System.Data.IDbConnection, Task> operation)
        {
            var connection = await GetConnectionAsync();
            await operation(connection);
        }
    }
}