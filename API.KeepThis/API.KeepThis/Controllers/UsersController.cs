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
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { Message = "Données invalides fournies.", Errors = errors });
            }

            try
            {
                var newUser = await _usersService.CreateUserAsync(userCreationDTO);
                return Ok(new { Message = "Compte créé avec succès.", UserId = newUser.IdUser });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erreur interne du serveur." });
            }
        }

        [HttpPut("update-username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDTO updateUsernameDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { Message = "Données invalides fournies.", Errors = errors });
            }

            try
            {
                await _usersService.UpdateUsernameAsync(updateUsernameDTO);
                return Ok(new { Message = "Nom d'utilisateur mis à jour avec succès." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erreur interne du serveur." });
            }
        }

        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDTO updateEmailDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { Message = "Données invalides fournies.", Errors = errors });
            }

            try
            {
                await _usersService.UpdateEmailAsync(updateEmailDTO);
                return Ok(new { Message = "Adresse e-mail mise à jour avec succès." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Utilisateur non trouvé." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erreur interne du serveur." });
            }
        }


        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { Message = "Données invalides fournies.", Errors = errors });
            }

            try
            {
                await _usersService.UpdatePasswordAsync(updatePasswordDTO);
                return Ok(new { Message = "Mot de passe mis à jour avec succès." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Utilisateur non trouvé." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Erreur interne du serveur." });
            }
        }




    }
}