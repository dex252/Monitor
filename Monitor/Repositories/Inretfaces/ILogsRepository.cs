using Monitor.Models.DTOs.Responses;
using Monitor.Models.Entities.Postgres;

namespace Monitor.Repositories.Inretfaces
{
    public interface ILogsRepository
    {
        Task<IEnumerable<LogResponse>> GetAllLogsAsync();
        Task<decimal> AddLogAsync(Log log);
        Task<LogResponse?> GetLogAsync(decimal id);
    }
}