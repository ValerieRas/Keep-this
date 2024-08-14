using API.KeepThis.Model;
using API.KeepThis.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.KeepThis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService UsersService)
        {
            _usersService = UsersService;
        }

        /// <summary>
        /// Creates a new user account
        /// </summary>
        /// <param name="user">The user's details for account creation</param>
        /// <returns>The created user or an error response</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([Bind("TempEmailUser, PasswordUser, NomUser")] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // The service handles the creation of the ID and Salt
                var newUser = await _usersService.CreateUserAsync(
                    user.TempEmailUser,
                    user.PasswordUser,
                    user.NomUser
                );

                return Ok(newUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
