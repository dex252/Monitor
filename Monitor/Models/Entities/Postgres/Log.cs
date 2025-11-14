using Dapper.Contrib.Extensions;
using Monitor.Models.DTOs.Requests;

namespace Monitor.Models.Entities.Postgres
{
    [Table("logs")]
    public class Log
    {
        [Key]
        public decimal Id { get; set; }
        public decimal UserId { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string QueryText { get; set; }
        public string StackTrace { get; set; }
        public string Application { get; set; }
        public string IpAddress { get; set; }

        public Log(CreateLogRequest logDto)
        {
            UserId = logDto.UserID;
            ErrorType = logDto.ErrorType.ToString();
            ErrorMessage = logDto.ErrorMessage;
            QueryText = logDto.QueryText;
            StackTrace = logDto.StackTrace;
            Application = logDto.Application.ToString();;
            IpAddress = logDto.IpAddress;
        }
        public Log()
        {
            
        }
    }
}
