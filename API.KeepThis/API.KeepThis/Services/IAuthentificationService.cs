using API.KeepThis.Model;
using API.KeepThis.Model.DTO;

namespace API.KeepThis.Services
{
    public interface IAuthentificationService
    {
        string GenerateJwtToken(User user);
        Task<LoginUserDto> AuthenticateUserAsync(LoginDto request);
    }
}
