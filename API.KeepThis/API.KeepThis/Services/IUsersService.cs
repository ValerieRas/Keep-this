using API.KeepThis.Model;
using API.KeepThis.Model.DTO;
using System.Threading.Tasks;

namespace API.KeepThis.Services
{
    public interface IUsersService
    {
        Task<User> CreateUserAsync(UserCreationDTO userCreationDTO);
    }
}
