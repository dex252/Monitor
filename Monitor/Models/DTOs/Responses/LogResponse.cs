using Monitor.Models.Entities.Postgres;

namespace Monitor.Models.DTOs.Responses
{
    public class LogResponse: Log
    {
        public string UserName { get;set;}

        public string Email {get;set;}
    }
}