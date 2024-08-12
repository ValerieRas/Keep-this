using API.KeepThis.Model;

namespace API.KeepThis.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetByEmailAsync(string email);
    }
}
