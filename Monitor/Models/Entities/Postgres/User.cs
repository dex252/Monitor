using Dapper.Contrib.Extensions;

namespace Monitor.Models.Entities.Postgres
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}