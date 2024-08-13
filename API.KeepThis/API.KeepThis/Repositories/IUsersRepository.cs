using API.KeepThis.Model;

namespace API.KeepThis.Repositories
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Retrieves a user from the database by their certified or temporary email.
        /// </summary>
        /// <param name="email">The email to search for (either certified or temporary).</param>
        /// <returns>
        /// The user entity if found; otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the email is null, empty, or consists only of white-space characters.</exception>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Adds a new user to the database asynchronously.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddUserAsync(User user);
    }
}
