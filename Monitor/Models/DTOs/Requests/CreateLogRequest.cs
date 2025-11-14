using Monitor.Enums;

namespace Monitor.Models.DTOs.Requests
{
    public class CreateLogRequest
    {
        public decimal UserID { get; set; }
        public ErrorType ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string QueryText { get; set; }
        public string StackTrace { get; set; }
        public Application Application { get; set; }
        public string IpAddress { get; set; }

        public CreateLogRequest()
        {
            
        }
    }
}