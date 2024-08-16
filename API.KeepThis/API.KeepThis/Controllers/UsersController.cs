using API.KeepThis.Model.DTO;
using API.KeepThis.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut("update-username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDTO updateUsernameDTO)
        {
            try
            {
                await _usersService.UpdateUsernameAsync(updateUsernameDTO.UserId, updateUsernameDTO.NewUsername);
                return Ok(new { Message = "Nom d'utilisateur mis à jour avec succès." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Utilisateur non trouvé." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erreur interne du serveur." });
            }
        }
    }
}
