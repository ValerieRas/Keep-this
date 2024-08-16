using API.KeepThis.Model;
using API.KeepThis.Model.DTO;
using Microsoft.AspNetCore.Identity.Data;
using System.Threading.Tasks;

namespace API.KeepThis.Services
{
    public interface IAuthentificationService
    {
        string GenerateJwtToken(User user);
        Task<LoginUserDto> AuthenticateUserAsync(LoginDto request);
    }
}
