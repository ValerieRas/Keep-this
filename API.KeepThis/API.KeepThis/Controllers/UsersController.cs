using API.KeepThis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.KeepThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _UsersService;
        public UsersController(IUsersService UsersService)
        {
            _UsersService = UsersService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _UsersService.AuthenticateUserAsync(request);

            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
