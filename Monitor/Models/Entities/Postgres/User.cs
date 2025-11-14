using Dapper.Contrib.Extensions;
using Monitor.Models.DTOs.Requests;

namespace Monitor.Models.Entities.Postgres
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public User(CreateUserRequest userDto)
        {
            Name = userDto.Name;
            Email = userDto.Email;
        }

        public User()
        {
            
        }
    }
}