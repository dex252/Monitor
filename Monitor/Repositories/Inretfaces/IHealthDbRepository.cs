namespace Monitor.Repositories.Inretfaces
{
    public interface IHealthDbRepository
    {
        Task<bool> CheckConnection();
    }
}