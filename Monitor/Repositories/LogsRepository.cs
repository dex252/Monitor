using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitor.Factories;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Repositories
{
    public class LogsRepository : BaseRepository, ILogsRepository
    {
        public LogsRepository(IPostgresConnection connection): base(connection)
        {
            
        }
    }
}