using API.KeepThis.Model;
using System.Threading.Tasks;


namespace API.KeepThis.Services
{
    public interface IUsersService
    {
        /// <summary>
        /// Create a new user asynchronously.
        /// </summary>
        /// <param name="tempEmailUser">The user's temporary email</param>
        /// <param name="passwordUser">The user's password</param>
        /// <param name="nomUser">The user's name</param>
        /// <returns>The created User entity</returns>
        Task<User> CreateUserAsync(string tempEmailUser, string passwordUser, string nomUser);
    }
}
