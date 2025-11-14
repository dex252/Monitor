namespace Monitor.Models.DTOs.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public CreateUserRequest()
        {
            
        }
    }
}