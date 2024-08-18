using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Model.DTO;
using API.KeepThis.Repositories;

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
            // Check if the email is already used
            var existingUser = await _usersRepository.GetByEmailAsync(userCreationDTO.UserEmail);
            if (existingUser != null)
            {
                throw new InvalidOperationException("L'adresse e-mail est déjà utilisée.");
            }

            // Map the DTO to the User entity
            var newUser = MapToUser(userCreationDTO);

            // Save the user to the database
            await _usersRepository.AddUserAsync(newUser);

            return newUser;
        }

        public async Task UpdateUsernameAsync(UpdateUsernameDTO updateUsernameDTO)
        {
            // Find the user by their ID
            var existingUser = await _usersRepository.GetByIdAsync(updateUsernameDTO.UserId);

            // If the user is not found, throw an exception
            if (existingUser == null)
            {
                throw new KeyNotFoundException("Utilisateur non trouvé.");
            }

            // Update the user's username
            existingUser.NomUser = updateUsernameDTO.NewUsername;

            // Send the updated user to the repository to save changes
            await _usersRepository.UpdateUsernameAsync(existingUser);
        }





        private User MapToUser(UserCreationDTO dto)
        {
            // Generate the salt using the password security helper
            string salt = _passwordSecurity.GenerateSalt();

            // Create and return a new User object mapped from the DTO
            return new User
            {
                IdUser = Guid.NewGuid().ToString(), // Generate a new GUID for IdUser
                TempEmailUser = dto.UserEmail,
                NomUser = dto.Username,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified), // Convert to Unspecified
                IsActive = true,
                SaltUser = salt, // Assign the generated salt
                PasswordUser = _passwordSecurity.HashPassword(dto.UserPassword, salt) // Hash the password using the salt
            };
        }
    }
}