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
            await _usersRepository.CreateUserAsync(newUser);

            return newUser;
        }

        public async Task UpdateUsernameAsync(UpdateUsernameDTO updateUsernameDTO)
        {
            // Find the user by their ID
            var existingUser = await _usersRepository.GetByIdAsync(updateUsernameDTO.UserId) ?? throw new KeyNotFoundException("Utilisateur non trouvé.");

            // Update the user's username
            existingUser.NomUser = updateUsernameDTO.NewUsername;

            // Save the changes to the database
            await _usersRepository.UpdateUserAsync(existingUser);
        }

        public async Task UpdateEmailAsync(UpdateEmailDTO updateEmailDTO)
        {
            // Retrieve the user by their ID
            var user = await _usersRepository.GetByIdAsync(updateEmailDTO.UserId) ?? throw new KeyNotFoundException("Utilisateur non trouvé.");

            // Check if the new email is the same as the current email
            if (user.TempEmailUser == updateEmailDTO.NewEmail)
            {
                throw new InvalidOperationException("Le nouvel e-mail ne peut être identique à l'e-mail actuel.");
            }

            // Check if the new email is already in use
            var existingUser = await _usersRepository.GetByEmailAsync(updateEmailDTO.NewEmail);
            if (existingUser != null && existingUser.IdUser != user.IdUser)
            {
                throw new InvalidOperationException("L'adresse e-mail est déjà utilisée par un autre utilisateur.");
            }

            // Update the user's email
            user.TempEmailUser = updateEmailDTO.NewEmail;

            // Save the changes to the database
            await _usersRepository.UpdateUserAsync(user);
        }

        public async Task UpdatePasswordAsync(UpdatePasswordDTO updatePasswordDTO)
        {
            // Retrieve the user by their ID
            var user = await _usersRepository.GetByIdAsync(updatePasswordDTO.UserId) ?? throw new KeyNotFoundException("Utilisateur non trouvé.");

            // Verify the current password
            bool isCurrentPasswordValid = _passwordSecurity.VerifyPassword(user.PasswordUser, updatePasswordDTO.CurrentPassword, user.SaltUser);

            if (!isCurrentPasswordValid)
            {
                throw new UnauthorizedAccessException("Le mot de passe actuel est incorrect.");
            }

            // Hash the new password
            string newHashedPassword = _passwordSecurity.HashPassword(updatePasswordDTO.NewPassword, user.SaltUser);

            // Update the user's password
            user.PasswordUser = newHashedPassword;

            // Save the changes to the database
            await _usersRepository.UpdateUserAsync(user);
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