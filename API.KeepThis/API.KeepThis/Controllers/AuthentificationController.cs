using API.KeepThis.Model.DTO;
using API.KeepThis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.KeepThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {

        private readonly IAuthentificationService _AuthentificationService;
        public AuthentificationController(IAuthentificationService UsersService)
        {
            _AuthentificationService = UsersService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var user = await _AuthentificationService.AuthenticateUserAsync(request);
                //var token = _AuthentificationService.GenerateJwtToken(user);

                //return Ok(new { Token = token });

                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Return 401 Unauthorized status code with error message
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                // You might want to log the exception here
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // GET endpoint to generate and send the email verification token
        [HttpGet("token-verification-email")]
        public async Task<IActionResult> GetTokenVerificationEmail([FromQuery] string email)
        {

            // Generate the verification token
            var token = _AuthentificationService.GenerateEmailVerificationToken(email);

            // Return the token
            return Ok(new { Token = token });
        }


        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody]string token)
        {
            var result = await _AuthentificationService.VerifyEmailAsync(token);

            if (result)
            {
                return Ok("Email verified successfully!");
            }
            else
            {
                return BadRequest("Invalid or expired token.");
            }
        }


    }
}
