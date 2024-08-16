using API.KeepThis.Model;

namespace API.KeepThis.Repositories
{
    public interface IUsersRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task UpdateUsernameAsync(string userId, string newUsername);

    }
}
