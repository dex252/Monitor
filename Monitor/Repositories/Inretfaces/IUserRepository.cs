using Monitor.Models.Entities.Postgres;

namespace Monitor.Repositories.Inretfaces
{
    public interface IUserRepository
    {
        Task<decimal> AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}