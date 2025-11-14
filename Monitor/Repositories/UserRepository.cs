using Dapper;
using Monitor.Factories;
using Monitor.Models.Entities.Postgres;
using Monitor.Repositories.Inretfaces;

namespace Monitor.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IPostgresConnection connection): base(connection)
        {
            
        }

        public async Task<decimal> AddUserAsync(User user)
        {
            const string sql = @"INSERT INTO users (name,email)
                                VALUES (@Name, @Email)
                                RETURNING id;";
            
            return await ExecuteAsync(async connection =>
            {
                var userId = await connection.ExecuteScalarAsync<decimal>(sql, new
                {
                    user.Name,
                    user.Email,
                });

                return userId;
            });
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            const string sql = @"
                select 
                    id, 
                    name,
                    email,
                    created_at as CreatedAt
                from 
                    users";
            return await ExecuteAsync(async connection =>
            {
                var users = await connection.QueryAsync<User>(sql);
                return users;
            });
        }
    }
}