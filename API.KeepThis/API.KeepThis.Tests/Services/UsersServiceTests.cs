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
            _mockUsersRepository.Setup(repo => repo.UpdateUsernameAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var updateDto = new UpdateUsernameDTO { UserId = userId, NewUsername = newUsername };

            // Act
            await _usersService.UpdateUsernameAsync(updateDto);

            // Assert
            _mockUsersRepository.Verify(repo => repo.UpdateUsernameAsync(It.Is<User>(u => u.NomUser == newUsername)), Times.Once);
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
    }
}
