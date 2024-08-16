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
            // Validate input
            if (string.IsNullOrWhiteSpace(userCreationDTO.TempEmailUser))
            {
                throw new ArgumentException("Le champ 'e-mail' ne peut pas être vide.", nameof(userCreationDTO.TempEmailUser));
            }

            if (string.IsNullOrWhiteSpace(userCreationDTO.PasswordUser))
            {
                throw new ArgumentException("Le champ 'mot de passe' ne peut pas être vide.", nameof(userCreationDTO.PasswordUser));
            }

            if (string.IsNullOrWhiteSpace(userCreationDTO.NomUser))
            {
                throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide.", nameof(userCreationDTO.NomUser));
            }

            // Check if the email is already used
            var existingUser = await _usersRepository.GetByEmailAsync(userCreationDTO.TempEmailUser);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Cette adresse e-mail est déjà utilisée.");
            }

            // Map the DTO to the User entity
            var newUser = MapToUser(userCreationDTO);

            // Save the user to the database
            await _usersRepository.AddUserAsync(newUser);

            return newUser;
        }


        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide.", nameof(newUsername));
            }

            var user = await _usersRepository.GetByEmailAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("Utilisateur non trouvé.");
            }

            await _usersRepository.UpdateUsernameAsync(userId, newUsername);
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
