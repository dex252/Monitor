using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitor.Enums;
using Monitor.Models.DTOs.Requests;
using Monitor.Models.Entities.Postgres;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Services
{
    public class BackgroundGenerateLogService : BackgroundService
    {
        private bool isActive = true;
        private int UsersCount { get; set; }
        private DateTime SnapshotTime { get; set; }
        private int SNAPSHOT_DIFFERENT_TIME = 300;
        private readonly ILogsRepository _logsRepository;
        private readonly IUserRepository _userRepository;
        private readonly Random Random = new Random();

        private static readonly string[] _errorPrefixes = { "Failed to", "Unable to", "Error while", "Exception in", "Timeout during", "Invalid", "Missing", "Unexpected" };
        private static readonly string[] _errorSubjects = { "process request", "connect to database", "execute query", "validate input", "serialize data", "deserialize response", "authenticate user", "authorize request", "load configuration", "save changes" };
        private static readonly string[] _sqlKeywords = { "SELECT", "INSERT", "UPDATE", "DELETE", "FROM", "WHERE", "JOIN", "ORDER BY", "GROUP BY", "HAVING" };
        private static readonly string[] _tableNames = { "users", "logs", "products", "orders", "customers", "settings", "permissions", "categories" };
        private readonly ConcurrentDictionary<decimal, User> _users;
        public BackgroundGenerateLogService(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            _logsRepository = scope.ServiceProvider.GetRequiredService<ILogsRepository>();
            _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            _users = new ConcurrentDictionary<decimal, User>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await UpdateUsersCacheAsync();

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    if (!isActive)
                    {
                        continue;
                    }

                    var userId = await GetRandomUserAsync();
                    var generatedLog = GenerateLog(userId);
                    await InsertLogInDb(generatedLog);
                    await Task.Delay(Random.Next(1000, 5000), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    //приложение останавливается
                    await Task.Delay(5000, stoppingToken);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await Task.Delay(5000, stoppingToken);
                }
                finally
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }


        }

        private async Task InsertLogInDb(CreateLogRequest logDto)
        {
            var log = new Log(logDto);
            await _logsRepository.AddLogAsync(log);
        }

        private CreateLogRequest GenerateLog(decimal userId)
        {
            return new CreateLogRequest
            {
                UserID = userId,
                ErrorType = GetRandomErrorType(),
                ErrorMessage = GenerateRandomErrorMessage(),
                QueryText = GenerateRandomQueryText(),
                StackTrace = GenerateRandomStackTrace(),
                Application = GetRandomApplication(),
                IpAddress = GenerateRandomIpAddress()
            };
        }

        private ErrorType GetRandomErrorType()
        {
            var values = Enum.GetValues(typeof(ErrorType));
            return (ErrorType)values.GetValue(Random.Next(values.Length));
        }

        private Application GetRandomApplication()
        {
            var values = Enum.GetValues(typeof(Application));
            return (Application)values.GetValue(Random.Next(values.Length));
        }

        private string GenerateRandomErrorMessage()
        {
            var prefix = _errorPrefixes[Random.Next(_errorPrefixes.Length)];
            var subject = _errorSubjects[Random.Next(_errorSubjects.Length)];
            var details = GenerateRandomString(20, 50);

            return $"{prefix} {subject}: {details}";
        }

        private string GenerateRandomQueryText()
        {
            var keyword = _sqlKeywords[Random.Next(_sqlKeywords.Length)];
            var table = _tableNames[Random.Next(_tableNames.Length)];
            var condition = GenerateRandomString(10, 30).Replace(" ", "_");

            return $"{keyword} * FROM {table} WHERE {condition} = '{GenerateRandomString(5, 15)}'";
        }

        private string GenerateRandomStackTrace()
        {
            var methods = new List<string>();
            for (int i = 0; i < Random.Next(3, 8); i++)
            {
                var className = GenerateRandomString(8, 15) + "Service";
                var methodName = GenerateRandomString(6, 12) + "Async";
                methods.Add($"   at {className}.{methodName}() in /src/{className}.cs:line {Random.Next(1, 100)}");
            }

            return string.Join("\n", methods);
        }

        private string GenerateRandomIpAddress()
        {
            return $"{Random.Next(1, 255)}.{Random.Next(0, 255)}.{Random.Next(0, 255)}.{Random.Next(1, 255)}";
        }

        private string GenerateRandomString(int minLength, int maxLength)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
            var length = Random.Next(minLength, maxLength);

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private async Task<decimal> GetRandomUserAsync()
        {
            var currentTime = DateTime.UtcNow;
            if ((currentTime - SnapshotTime).TotalSeconds > SNAPSHOT_DIFFERENT_TIME)
            {
                await UpdateUsersCacheAsync();
            }

            var randomIndex = Random.Next(0, UsersCount);
            var randomUserId = _users.Keys.ElementAt(randomIndex);
            return randomUserId;
        }

        private async Task UpdateUsersCacheAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            _users.Clear();
            foreach (var user in users)
            {
                _users.AddOrUpdate(user.Id, user, (e, v) => v);
            }

            UsersCount = _users.Count();
            SnapshotTime = DateTime.UtcNow;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public bool Start()
        {
            if (isActive)
            {
                return false;
            }

            isActive = true;
            return true;
        }

        public bool Stop()
        {
            if (!isActive)
            {
                return false;
            }

            isActive = false;
            return true;
        }

        public bool Status()
        {
            return isActive;
        }
    }
}
