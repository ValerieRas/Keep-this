using API.KeepThis.Model;
using Microsoft.AspNetCore.Identity.Data;

namespace API.KeepThis.Services
{
    public interface IAuthentificationService
    {
        string GenerateJwtToken(User user);
        Task<User> AuthenticateUserAsync(LoginRequest request);
    }
}
