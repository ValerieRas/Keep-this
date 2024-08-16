using API.KeepThis.Model;
using API.KeepThis.Model.DTO;

namespace API.KeepThis.Services
{
    public interface IUsersService
    {
        Task<User> CreateUserAsync(UserCreationDTO userCreationDTO);
        Task UpdateUsernameAsync(string userId, string newUsername);
    }
}
