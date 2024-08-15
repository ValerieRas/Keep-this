using API.KeepThis.Model;

namespace API.KeepThis.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}
