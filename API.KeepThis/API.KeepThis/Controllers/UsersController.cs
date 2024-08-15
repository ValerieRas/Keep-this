using API.KeepThis.Model.DTO;
using API.KeepThis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.KeepThis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("register-account")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDTO userCreationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _usersService.CreateUserAsync(userCreationDTO);

            return Ok(createdUser);
        }

    }
}
