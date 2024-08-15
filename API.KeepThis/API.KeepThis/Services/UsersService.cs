using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Model.DTO;
using API.KeepThis.Repositories;
using System;
using System.Threading.Tasks;

namespace API.KeepThis.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordSecurity _passwordSecurity;

        public UsersService(IUsersRepository userRepository, IPasswordSecurity passwordSecurity)
        {
            _usersRepository = userRepository;
            _passwordSecurity = passwordSecurity;
        }

        public async Task<User> CreateUserAsync(UserCreationDTO userCreationDTO)
        {
            // Map the DTO to the User entity
            var newUser = MapToUser(userCreationDTO);

            // Save the user to the database
            await _usersRepository.AddUserAsync(newUser);

            return newUser;
        }

        private User MapToUser(UserCreationDTO dto)
        {
            // Generate the salt using the password security helper
            string salt = _passwordSecurity.GenerateSalt();

            // Create and return a new User object mapped from the DTO
            return new User
            {
                IdUser = Guid.NewGuid().ToString(), // Generate a new GUID for IdUser
                TempEmailUser = dto.TempEmailUser,
                NomUser = dto.NomUser,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified), // Convert to Unspecified
                IsActive = true,
                SaltUser = salt, // Assign the generated salt
                PasswordUser = _passwordSecurity.HashPassword(dto.PasswordUser, salt) // Hash the password using the salt
            };
        }


    }
}
