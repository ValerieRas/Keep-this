using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Model.DTO;
using API.KeepThis.Repositories;
using API.KeepThis.Services;
using Moq;
using Xunit;


namespace API.KeepThis.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly UsersService _usersService = null!;
        private readonly Mock<IUsersRepository> _mockUsersRepository = null!;
        private readonly Mock<IPasswordSecurity> _mockPasswordSecurity = null!;

        public UsersServiceTests()
        {
            _mockUsersRepository = new Mock<IUsersRepository>();
            _mockPasswordSecurity = new Mock<IPasswordSecurity>();
            _usersService = new UsersService(_mockUsersRepository.Object, _mockPasswordSecurity.Object);
        }

        #region Create User tests

        [Fact]
        public async Task CreateUserAsync_ExistingEmail_ThrowsException()
        {
            // Arrange
            var userCreationDTO = new UserCreationDTO
            {
                UserEmail = "existing@example.com",
                UserPassword = "ValidPassword123!",
                Username = "Existing User"
            };

            // Mock the GetByEmailAsync method to return an existing user
            _mockUsersRepository.Setup(repo => repo.GetByEmailAsync(userCreationDTO.UserEmail))
                .ReturnsAsync(new User { TempEmailUser = userCreationDTO.UserEmail });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _usersService.CreateUserAsync(userCreationDTO));

            Assert.Equal("L'adresse e-mail est déjà utilisée.", exception.Message);
        }

        [Fact]
        public async Task CreateUserAsync_SaveToDatabaseFails_ThrowsException()
        {
            // Arrange
            var userCreationDTO = new UserCreationDTO
            {
                UserEmail = "newuser@example.com",
                UserPassword = "ValidPassword123!",
                Username = "New User"
            };

            // Mock the GetByEmailAsync method to return null (email not in use)
            _ = _mockUsersRepository.Setup(repo => repo.GetByEmailAsync(userCreationDTO.UserEmail))
                .ReturnsAsync((User)null);

            // Mock the CreateUserAsync method to throw an exception
            _mockUsersRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Database save failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _usersService.CreateUserAsync(userCreationDTO));

            Assert.Equal("Database save failed", exception.Message);
        }

        #endregion

        #region Username Update Tests

        [Fact]
        public async Task UpdateUsernameAsync_ValidUserId_UpdatesUsernameSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var newUsername = "newUsername123";
            var existingUser = new User { IdUser = userId, NomUser = "OldUsername" };

            // Setup the mock to return the existing user when GetByIdAsync is called
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Setup the mock to simulate the update operation
            _mockUsersRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var updateDto = new UpdateUsernameDTO { UserId = userId, NewUsername = newUsername };

            // Act
            await _usersService.UpdateUsernameAsync(updateDto);

            // Assert
            _mockUsersRepository.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.NomUser == newUsername)), Times.Once);
        }

        [Fact]
        public async Task UpdateUsernameAsync_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var newUsername = "newUsername123";

            // Setup the mock to return null when GetByIdAsync is called with a non-existing user ID
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            var updateDto = new UpdateUsernameDTO { UserId = userId, NewUsername = newUsername };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _usersService.UpdateUsernameAsync(updateDto));
        }

        #endregion

        #region Email Update Tests

        [Fact]
        public async Task UpdateEmailAsync_ValidUserId_UpdatesEmailSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var newEmail = "newemail@example.com";
            var existingUser = new User { IdUser = userId, TempEmailUser = "oldemail@example.com" };

            // Setup the mock to return the existing user when GetByIdAsync is called
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            await _usersService.UpdateEmailAsync(new UpdateEmailDTO { UserId = userId, NewEmail = newEmail });

            // Assert
            _mockUsersRepository.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.TempEmailUser == newEmail)), Times.Once);
        }

        [Fact]

        public async Task UpdateEmailAsync_SameEmail_ThrowsInvalidOperationException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var newEmail = "sameemail@example.com";
            var existingUser = new User { IdUser = userId, TempEmailUser = newEmail };

            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _usersService.UpdateEmailAsync(new UpdateEmailDTO { UserId = userId, NewEmail = newEmail }));

            Assert.Equal("Le nouvel e-mail ne peut être identique à l'e-mail actuel.", exception.Message);
        }

        [Fact]
        public async Task UpdateEmailAsync_EmailAlreadyInUse_ThrowsInvalidOperationException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var newEmail = "existingemail@example.com";
            var existingUser = new User { IdUser = userId, TempEmailUser = "oldemail@example.com" };
            var otherUser = new User { IdUser = Guid.NewGuid().ToString(), TempEmailUser = newEmail }; // Another user with the same email

            // Setup the mock to return the existing user when GetByIdAsync is called
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Setup the mock to return another user when GetByEmailAsync is called with the new email
            _mockUsersRepository.Setup(repo => repo.GetByEmailAsync(newEmail)).ReturnsAsync(otherUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _usersService.UpdateEmailAsync(new UpdateEmailDTO { UserId = userId, NewEmail = newEmail }));

            Assert.Equal("L'adresse e-mail est déjà utilisée par un autre utilisateur.", exception.Message);
        }


        #endregion

        #region Password Update Tests

        [Fact]
        public async Task UpdatePasswordAsync_ValidUserIdAndPassword_UpdatesPasswordSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var currentPassword = "CurrentPassword123!";
            var newPassword = "NewPassword123!";
            var salt = "GeneratedSalt";
            var existingUser = new User { IdUser = userId, PasswordUser = "HashedPassword123", SaltUser = salt };

            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockPasswordSecurity.Setup(ps => ps.VerifyPassword(existingUser.PasswordUser, currentPassword, salt)).Returns(true);
            _mockPasswordSecurity.Setup(ps => ps.HashPassword(newPassword, salt)).Returns("NewHashedPassword");

            // Act
            await _usersService.UpdatePasswordAsync(new UpdatePasswordDTO { UserId = userId, CurrentPassword = currentPassword, NewPassword = newPassword });

            // Assert
            _mockUsersRepository.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.PasswordUser == "NewHashedPassword")), Times.Once);
        }

        [Fact]
        public async Task UpdatePasswordAsync_IncorrectCurrentPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var currentPassword = "WrongPassword";
            var newPassword = "NewPassword123!";
            var existingUser = new User { IdUser = userId, PasswordUser = "HashedPassword123", SaltUser = "GeneratedSalt" };

            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockPasswordSecurity.Setup(ps => ps.VerifyPassword(existingUser.PasswordUser, currentPassword, existingUser.SaltUser)).Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _usersService.UpdatePasswordAsync(new UpdatePasswordDTO { UserId = userId, CurrentPassword = currentPassword, NewPassword = newPassword }));

            Assert.Equal("Le mot de passe actuel est incorrect.", exception.Message);
        }

        #endregion
    }
}
