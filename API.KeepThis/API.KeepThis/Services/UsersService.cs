using System;
using System.Threading.Tasks;
using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Repositories;

namespace API.KeepThis.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordSecurity _passwordSecurity;

        public UsersService(IUsersRepository usersRepository, IPasswordSecurity passwordSecurity)
        {
            _usersRepository = usersRepository;
            _passwordSecurity = passwordSecurity;
        }

        /// <summary>
        /// Create a new user asynchronously.
        /// </summary>
        /// <param name="tempEmailUser">The user's temporary email</param>
        /// <param name="passwordUser">The user's password</param>
        /// <param name="nomUser">The user's name</param>
        /// <returns>The created User entity</returns>
        /// <exception cref="ArgumentException">Thrown when the email or password is invalid</exception>
        public async Task<User> CreateUserAsync(string tempEmailUser, string passwordUser, string nomUser)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(tempEmailUser))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(tempEmailUser));
            }

            if (string.IsNullOrWhiteSpace(passwordUser))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(passwordUser));
            }

            if (string.IsNullOrWhiteSpace(nomUser))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(nomUser));
            }

            // Create a new user object
            var newUser = new User
            {
                TempEmailUser = tempEmailUser,
                NomUser = nomUser,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Hash the password using the generated salt
            newUser.PasswordUser = _passwordSecurity.HashPassword(passwordUser, newUser.SaltUser);

            try
            {
                await _usersRepository.AddUserAsync(newUser);
            }
            catch (Exception ex)
            {
                // Log the exception message or stack trace
                Console.WriteLine($"Error saving user: {ex.Message}");
                throw;
            }


            return newUser;
        }
    }
}
