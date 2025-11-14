using Dapper;
using Monitor.Factories;
using Monitor.Models.DTOs.Responses;
using Monitor.Models.Entities.Postgres;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Repositories
{
    public class LogsRepository : BaseRepository, ILogsRepository
    {
        public LogsRepository(IPostgresConnection connection): base(connection)
        {
            
        }

        public async Task<decimal> AddLogAsync(Log log)
        {
            const string sql = 
            @"INSERT INTO logs 
                (user_id,
                error_type,
                error_message,
                query_text,
                stack_trace,
                application,
                ip_address)
            VALUES 
                (@UserID,
                @ErrorType,
                @ErrorMessage,
                @QueryText,
                @StackTrace,
                @Application,
                @IpAddress)
            RETURNING id;";
            
            return await ExecuteAsync(async connection =>
            {
                var logId = await connection.ExecuteScalarAsync<decimal>(sql, new
                {
                    log.UserId,
                    log.ErrorType,
                    log.ErrorMessage,
                    log.QueryText,
                    log.StackTrace,
                    log.Application,
                    log.IpAddress,
                });

                return logId;
            });
        }

        public async Task<IEnumerable<LogResponse>> GetAllLogsAsync()
        {
            const string sql = @"
            select
                l.id as Id,
                l.user_id as UserId, 
                l.error_type as ErrorType,
                l.error_message as ErrorMessage,
                l.query_text as QueryText,
                l.stack_trace as StackTrace,
                l.application as Application,
                l.ip_address as IpAddress,
                l.created_at as CreatedAt,
                u.name as UserName,
                u.email as Email
            from 
                logs l
            left join users u ON l.user_id = u.id";

            return await ExecuteAsync(async connection =>
            {
                var users = await connection.QueryAsync<LogResponse>(sql);
                return users;
            });
        }

        public async Task<LogResponse?> GetLogAsync(decimal id)
        {
            const string sql = @"
            select
                l.id as Id,
                l.user_id as UserId, 
                l.error_type as ErrorType,
                l.error_message as ErrorMessage,
                l.query_text as QueryText,
                l.stack_trace as StackTrace,
                l.application as Application,
                l.ip_address as IpAddress,
                l.created_at as CreatedAt,
                u.name as UserName,
                u.email as Email
            from 
                logs l
            left join users u ON l.user_id = u.id
            where 
                l.id=@Id";
            return await ExecuteAsync(async connection =>
            {
                var log = await connection.QueryFirstOrDefaultAsync<LogResponse>(sql, new { Id = id });
                return log;
            });
        }

    }
}