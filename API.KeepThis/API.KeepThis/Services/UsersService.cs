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
        private readonly IPasswordHasher _passwordHasher;

        public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
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
            // Validate the email and password
            if (string.IsNullOrWhiteSpace(tempEmailUser))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(tempEmailUser));
            }

            if (string.IsNullOrWhiteSpace(passwordUser))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(passwordUser));
            }

            // Check if the email is already in use
            var existingUser = await _usersRepository.GetByEmailAsync(tempEmailUser);
            if (existingUser != null)
            {
                throw new ArgumentException("A user with this email already exists.");
            }

            // Hash the password
            string hashedPassword = _passwordHasher.HashPassword(passwordUser);

            // Create the new user
            var newUser = new User
            {
                IdUser = Guid.NewGuid().ToString(), // Generate a new unique ID
                TempEmailUser = tempEmailUser,
                PasswordUser = hashedPassword,
                NomUser = nomUser,
                CreatedAt = DateTime.UtcNow,
                IsActive = true // Set user as active by default
            };

            // Add the user to the database
            await _usersRepository.AddUserAsync(newUser);

            // Return the created user
            return newUser;
        }
    }
}
