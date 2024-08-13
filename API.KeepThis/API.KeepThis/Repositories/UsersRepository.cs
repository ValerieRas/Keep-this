using API.KeepThis.Data;
using API.KeepThis.Model;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly KeepThisDbContext _context;

        public UsersRepository(KeepThisDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user from the database by their certified or temporary email.
        /// </summary>
        /// <param name="email">The email to search for (either certified or temporary).</param>
        /// <returns>
        /// The user entity if found; otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the email is null, empty, or consists only of white-space characters.</exception>
        public async Task<User?> GetByEmailAsync(string email)
        {
            // Validate the email input
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null, empty, or whitespace.", nameof(email));
            }

            // Search for the user by either CertifiedEmailUser or TemporaryEmail
            // Return null if nothing is found, throw exception if more than one
            return await _context.Users
                .SingleOrDefaultAsync(u => u.CertifiedEmailUser == email || u.TempEmailUser == email);
        }

        /// <summary>
        /// Add a new user to the database asynchronously.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddUserAsync(User user)
        {
            // Add the user entity to the Users DbSet
            await _context.Users.AddAsync(user);

            // Save the changes asynchronously to the database
            await _context.SaveChangesAsync();
        }
    }
}
