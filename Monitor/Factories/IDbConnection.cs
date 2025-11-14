namespace Monitor.Factories
{
    public interface IDbConnection
    {
        Task<System.Data.Common.DbConnection> GetConnectionAsync();
    }
}